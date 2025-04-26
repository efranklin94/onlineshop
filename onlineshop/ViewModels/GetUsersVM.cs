namespace onlineshop.ViewModels
{
    public class GetUsersVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}