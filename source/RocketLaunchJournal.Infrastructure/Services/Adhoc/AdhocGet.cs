using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using RocketLaunchJournal.Infrastructure.Dtos.Helpers;
using RocketLaunchJournal.Model.Adhoc;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Services.Adhoc
{
    public class AdhocGet : BaseService
    {
        /// <summary>
        /// Gets all reports available to user
        /// </summary>
        public async Task<IEnumerable<ReportDto>> GetReports()
        {
            return await (from x in db.RoleRestrictedReports(UserPermissionService)
                          let user = x.User
                          let userName = string.Concat(user.FirstName, " ", user.LastName!.Substring(0, 1), ".")
                          orderby x.Name
                          select new ReportDto()
                          {
                              ReportSourceId = x.ReportSourceId,
                              Name = x.Name,
                              IsShared = x.IsShared,
                              Data = x.Data,
                              UserId = x.UserId,
                              ReportId = x.ReportId,
                              UserName = userName
                          }).ToListAsync();
        }

        /// <summary>
        /// Gets all report sources
        /// </summary>
        public async Task<IEnumerable<ReportSourceDto>> GetReportSources()
        {
            return await (from x in db.ReportSources
                          orderby x.Name
                          select new ReportSourceDto()
                          {
                              ReportSourceId = x.ReportSourceId,
                              Name = x.Name,
                              SQLName = x.SQLName
                          }).ToListAsync();
        }

        /// <summary>
        /// Get columns for a report source
        /// </summary>
        /// <param name="dto">report source dto</param>
        /// <returns>ReportSourceColumnDto list</returns>
        public async Task<List<ReportSourceColumnDto>> GetReportSourceColumns(ReportSource dto)
        {
            var reportSource = await  db.ReportSources.FirstOrDefaultAsync(w => w.ReportSourceId == dto.ReportSourceId);
            using var cmd = new SqlCommand();
            cmd.Connection = db.Database.GetDbConnection() as SqlConnection;
            cmd.CommandText = $"Select top 1 * From {reportSource.SQLName} Where 1 = 0";
            var adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            var dataSet = new DataSet();
            adapter.Fill(dataSet);
            var columns = new List<ReportSourceColumnDto>();
            foreach (DataColumn item in dataSet.Tables[0].Columns)
            {
                columns.Add(new ReportSourceColumnDto() 
                {
                    Name = item.ColumnName,
                    TypeName = item.DataType.Name
                });
            }
            return columns;
        }

        /// <summary>
        /// Generates a report
        /// </summary>
        /// <param name="dto">report definition</param>
        /// <returns>list of data</returns>
        public async Task<ReportDataDto<object>?> GenerateReport(ReportDto dto)
        {
            var reportSource = await db.ReportSources.FirstOrDefaultAsync(w => w.ReportSourceId == dto.ReportSourceId);
            if (reportSource == null)
                return null;
            var report = new ReportDataDto<object>();
            var columns = GetReportSourceColumns(reportSource);
            //report.RemovedColumns = CheckColumns(dto, columns);
            if (dto.Columns.Count == 0)
                return report;

            var query = new SqlKata.Query(reportSource.SQLName)
            {
                IsDistinct = true
            };
            SelectClause(query, dto);
            GroupByClause(query, dto);
            OrderByClause(query, dto);
            var compiler = new SqlServerCompiler();
            var result = compiler.Compile(query);

            using var cmd = new SqlCommand();
            cmd.Connection = db.Database.GetDbConnection() as SqlConnection;
            cmd.CommandText = result.RawSql;
            var adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            var dataSet = new DataSet();
            adapter.Fill(dataSet);
            report.Data = dataSet.Tables[0].ToList().ToList();
            return report;
        }

        public async Task<FileDownload?> DownloadReport(ReportDto dto)
        {
            var report = await GenerateReport(dto);
            if (report == null)
                return null;

            var fileDownload = new FileDownload()
            {
                DownloadFilename = $"{dto.Name}_{DateTime.UtcNow:yyyyMMdd-hhmmss}.csv",
                FilePath = Path.GetTempFileName(),
                MimeType = "text/csv"
            };
            using var fileStream = new FileStream(fileDownload.FilePath, FileMode.Create);
            await report.Data.GenerateCSVAsync(fileStream, new IEnumerableExtensionOptions() { ShowHeaderRow = true });
            return fileDownload;
        }

        private static List<string> CheckColumns(ReportDto dto, List<ReportSourceColumnDto> columns)
        {
            var removedList = new List<string>();
            for (int i = dto.Columns.Count - 1; i >=0; i--)
            {
                if (!columns.Any(w=>w.Name == dto.Columns[i].Name))
                {
                    removedList.Add(dto.Columns[i].Name);
                    dto.Columns.RemoveAt(i);
                }
            }

            return removedList;
        }

        private static void SelectClause(SqlKata.Query query, ReportDto dto)
        {
            foreach (var item in dto.Columns.Where(w => w.InOutput).GroupBy(g => g.Aggregate == AggregateTypes.GroupBy).Select(s => new { s.Key, Columns = s.ToList() }))
            {
                if (item.Key)
                    query = query.Select(item.Columns.Select(s => s.Name).ToArray());
                else
                    query = query.SelectRaw(string.Join(',', item.Columns.Select(s => $"{s.Aggregate.GetShortDisplayName()}([{s.Name}]) as {s.ColumnName}").ToArray()));
            }
        }

        private static void GroupByClause(SqlKata.Query query, ReportDto dto)
        {
            // all using group by, don't need to have it.
            if (dto.Columns.All(w => w.Aggregate == AggregateTypes.GroupBy))
                return;

            query = query.GroupBy(dto.Columns.Where(w => w.Aggregate == AggregateTypes.GroupBy).Select(s => s.Name).ToArray());
        }

        private static void OrderByClause(SqlKata.Query query, ReportDto dto)
        {
            foreach (var item in dto.Columns.Where(w => w.Sort.HasValue && w.SortOrder.HasValue).OrderBy(o => o.SortOrder!.Value))
            {
                switch (item.Sort!.Value)
                {
                    case SortTypes.Ascending:
                        query = query.OrderBy(item.ColumnName);
                        break;
                    case SortTypes.Descending:
                        query = query.OrderByDesc(item.ColumnName);
                        break;
                }
            }
        }
    }
}
