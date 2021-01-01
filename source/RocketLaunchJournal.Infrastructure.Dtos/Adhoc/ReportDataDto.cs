using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Dtos.Adhoc
{
    public class ReportDataDto
    {
        public string Name { get; set; }
        public List<string> RemovedColumns { get; set; }
        public IEnumerable<Dictionary<string,object>> Data { get; set; }
    }
}
