using System.Collections.Generic;

namespace RocketLaunchJournal.Web.Client.Helpers;

public static class Constants
{
    public static readonly List<int> PageSizes = new List<int>() { 10, 15, 20, 30, 50 };
    public static readonly int DefaultPageSize = 15;
}
