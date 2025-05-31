namespace DomainService;

public interface ICurrentUser
{
    public string IPAddress { get; }
    public string Username { get; }
}
