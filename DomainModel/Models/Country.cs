using System.Text.Json.Serialization;

namespace onlineshop.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [JsonIgnore]
    public ICollection<City> Cities { get; set; } = new List<City>();
}
