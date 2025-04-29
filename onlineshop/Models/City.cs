namespace onlineshop.Models
{
    public class City
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Country { get; set; }
    }
}
