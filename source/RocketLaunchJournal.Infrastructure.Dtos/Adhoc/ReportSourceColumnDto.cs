using System.ComponentModel.DataAnnotations;

namespace RocketLaunchJournal.Infrastructure.Dtos.Adhoc;

public class ReportSourceColumnDto
{
  public string Name { get; set; } = default!;

  public string TypeName { get; set; } = default!;

  [Display(Name = "Output")]
  public bool InOutput { get; set; } = true;

  [Display(Name = "Sort Order")]
  public int? SortOrder { get; set; }

  public SortTypes? Sort { get; set; }

  public AggregateTypes Aggregate { get; set; }

  public int? FilterOperator { get; set; }

  public string? FilterGroup { get; set; }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;

    if (obj is ReportSourceColumnDto item)
    {
      return Name == item.Name && item.TypeName == TypeName && item.InOutput == InOutput && SortOrder == item.SortOrder &&
          item.Sort == Sort && Aggregate == item.Aggregate && FilterGroup == item.FilterGroup;
    }

    return false;
  }

  public string ColumnName
  {
    get
    {
      var sb = new System.Text.StringBuilder();
      sb.Append(Name);
      if (Aggregate != AggregateTypes.GroupBy)
        sb.Append($"_{Aggregate.ToString()}");

      return sb.ToString();
    }
  }
}
