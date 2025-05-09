namespace onlineshop.ViewModels;

public class EnumViewModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }

    public Dictionary<string, object> Infos { get; set; } = new();
}
