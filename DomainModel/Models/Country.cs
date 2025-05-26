using DomainModel.Models;
using System.Text.Json.Serialization;

namespace onlineshop.Models;

public class Country : BaseModel
{
    public string Name { get; set; } = string.Empty;
    [JsonIgnore]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
