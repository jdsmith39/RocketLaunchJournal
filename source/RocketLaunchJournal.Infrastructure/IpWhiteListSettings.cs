using System.Collections.Generic;

namespace RocketLaunchJournal.Infrastructure
{
    public class IpWhiteListSettings
    {
        public List<IpWhiteListItem> IpList { get; set; }

        public class IpWhiteListItem
        {
            public string Name { get; set; }
            public List<string> Values { get; set; }
        }
    }
}
