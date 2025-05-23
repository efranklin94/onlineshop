using DomainModel.Models;

namespace onlineshop.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string TrackingCode { get; set; } = string.Empty;
    public List<UserOption> Options { get; set; } = [];
    public List<UserTag> Tags { get; set; } = [];
}