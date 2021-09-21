using System.Collections.Generic;

namespace RocketLaunchJournal.Infrastructure.Dtos.Adhoc
{
    public class ReportDataDto<T>
    {
        public string Name { get; set; }
        //public List<string> RemovedColumns { get; set; }
        public List<Dictionary<string,T>> Data { get; set; }
    }
}
