namespace DomainModel.Models;
public class BackOfficeUser : BaseModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public DateTime? LastLoginAt { get; private set; }
    public List<BackOfficeUserRole> Roles { get; set; } = [];

    public void SetLastLoginAt()
    {
        LastLoginAt = DateTime.Now;
    }
}