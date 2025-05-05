namespace onlineshop.DTOs
{
    public class CreateOrUpdateUserDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
    }
}
