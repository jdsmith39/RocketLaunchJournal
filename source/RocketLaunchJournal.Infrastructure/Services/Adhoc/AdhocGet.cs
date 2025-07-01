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

namespace RocketLaunchJournal.Infrastructure.Services.Adhoc;

public class AdhocGet : BaseService
{
  /// <summary>
  /// Gets all reports available to user
  /// </summary>
  public Task<List<ReportDto>> GetReports()
  {
    return (from x in db.RoleRestrictedReports(UserPermissionService)
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
  public Task<List<ReportSourceDto>> GetReportSources()
  {
    return (from x in db.ReportSources
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
    var reportSource = await db.ReportSources.FirstOrDefaultAsync(w => w.ReportSourceId == dto.ReportSourceId);
    using var cmd = (SqlCommand)db.Database.GetDbConnection().CreateCommand();
    var isOpen = cmd.Connection.State == ConnectionState.Open;
    if (!isOpen)
      await cmd.Connection.OpenAsync();
    //cmd.Connection = db.Database.GetDbConnection() as SqlConnection;
    cmd.CommandText = $"Select top 1 * From {reportSource.SQLName} Where 1 = 0";
    var adapter = new SqlDataAdapter();
    adapter.SelectCommand = cmd;
    var dataSet = new DataSet();
    adapter.Fill(dataSet);
    if (!isOpen)
      await cmd.Connection.CloseAsync();

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
    if (dto.Columns.Count == 0)
      return report;

    var query = new SqlKata.Query(reportSource.SQLName)
    {
      IsDistinct = true
    };
    SelectClause(query, dto);
    GroupByClause(query, dto);
    WhereClause(query, dto);
    OrderByClause(query, dto);
    var compiler = new SqlServerCompiler();
    var result = compiler.Compile(query);

    using var cmd = (SqlCommand)db.Database.GetDbConnection().CreateCommand();
    var isOpen = cmd.Connection.State == ConnectionState.Open;
    if (!isOpen)
      await cmd.Connection.OpenAsync();

    cmd.CommandText = result.Sql;
    cmd.Parameters.AddRange(result.NamedBindings.Select(s => new SqlParameter(s.Key, s.Value)).ToArray());
    var adapter = new SqlDataAdapter();
    adapter.SelectCommand = cmd;
    var dataSet = new DataSet();
    adapter.Fill(dataSet);

    if (!isOpen)
      await cmd.Connection.CloseAsync();

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

  private static void WhereClause(SqlKata.Query query, ReportDto dto)
  {
    foreach (var item in dto.Columns.Where(w=> w.FilterOperator.HasValue))
    {
      // nothing to do continue
      if (item.TypeName.ToLower() != "boolean" && string.IsNullOrEmpty(item.FilterGroup))
        continue;

      switch (item.TypeName.ToLower())
      {
        case "string":
          var stringOperator = (RocketLaunchJournal.Model.Enums.StringOperators)item.FilterOperator!.Value;
          switch (stringOperator)
          {
            case Model.Enums.StringOperators.Contains:
              query = query.WhereContains(item.ColumnName, item.FilterGroup);
              break;
            case Model.Enums.StringOperators.StartsWith:
              query = query.WhereStarts(item.ColumnName, item.FilterGroup);
              break;
            case Model.Enums.StringOperators.EndsWith:
              query = query.WhereEnds(item.ColumnName, item.FilterGroup);
              break;
            case Model.Enums.StringOperators.Equals:
              query = query.Where(item.ColumnName, "=", item.FilterGroup);
              break;
            case Model.Enums.StringOperators.NotEquals:
              query = query.Where(item.ColumnName, "!=", item.FilterGroup);
              break;
          }
          break;
        case "int":
        case "decimal":
        case "double":
          var numericOperator = (RocketLaunchJournal.Model.Enums.NumericOperators)item.FilterOperator!.Value;
          switch (numericOperator)
          {
            case Model.Enums.NumericOperators.Equals:
              query = query.Where(item.ColumnName, "=", item.FilterGroup);
              break;
            case Model.Enums.NumericOperators.NotEquals:
              query = query.Where(item.ColumnName, "!=", item.FilterGroup);
              break;
            case Model.Enums.NumericOperators.GreaterThan:
              query = query.Where(item.ColumnName, ">", item.FilterGroup);
              break;
            case Model.Enums.NumericOperators.GreaterThanOrEqual:
              query = query.Where(item.ColumnName, ">=", item.FilterGroup);
              break;
            case Model.Enums.NumericOperators.LessThan:
              query = query.Where(item.ColumnName, "<", item.FilterGroup);
              break;
            case Model.Enums.NumericOperators.LessThanOrEqual:
              query = query.Where(item.ColumnName, "<=", item.FilterGroup);
              break;
          }
          break;
        case "datetime":
          var dateOperators = (RocketLaunchJournal.Model.Enums.DateTimeOperators)item.FilterOperator!.Value;
          switch (dateOperators)
          {
            case Model.Enums.DateTimeOperators.Equals:
              query = query.Where(item.ColumnName, "=", item.FilterGroup);
              break;
            case Model.Enums.DateTimeOperators.NotEquals:
              query = query.Where(item.ColumnName, "!=", item.FilterGroup);
              break;
            case Model.Enums.DateTimeOperators.GreaterThan:
              query = query.Where(item.ColumnName, ">", item.FilterGroup);
              break;
            case Model.Enums.DateTimeOperators.GreaterThanOrEqual:
              query = query.Where(item.ColumnName, ">=", item.FilterGroup);
              break;
            case Model.Enums.DateTimeOperators.LessThan:
              query = query.Where(item.ColumnName, "<", item.FilterGroup);
              break;
            case Model.Enums.DateTimeOperators.LessThanOrEqual:
              query = query.Where(item.ColumnName, "<=", item.FilterGroup);
              break;
          }
          break;
        case "boolean":
          var boolOperators = (RocketLaunchJournal.Model.Enums.BooleanOperators)item.FilterOperator!.Value;
          switch (boolOperators)
          {
            case Model.Enums.BooleanOperators.IsTrue:
              query = query.WhereTrue(item.ColumnName);
              break;
            case Model.Enums.BooleanOperators.IsFalse:
              query = query.WhereFalse(item.ColumnName);
              break;
          }
          break;
      }
    }
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
