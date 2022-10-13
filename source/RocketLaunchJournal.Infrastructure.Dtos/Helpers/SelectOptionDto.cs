namespace RocketLaunchJournal.Infrastructure.Dtos.Helpers;
public class SelectOptionDto<T>
{
    public T Value { get; set; } = default!;
    public string Text { get; set; } = default!;
    public string? Type { get; set; }
    public string? Description { get; set; }
    public bool isSelected { get; set; }
}

