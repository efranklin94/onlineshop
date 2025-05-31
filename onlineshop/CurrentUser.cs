using DomainService;
using System.Security.Claims;

namespace API;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string IPAddress => httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

    public string Username => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    public bool HasGodAccess => bool.Parse(httpContextAccessor.HttpContext?.User.FindFirstValue("HasGodAccess") ?? "False");
}
