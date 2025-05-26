using DomainModel.Models;
using onlineshop.Enums;

namespace onlineshop.Models
{
    public class City : BaseModel
    {
        public required string Name { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public CitiesType Type { get; set; }
    }
}
