using DomainModel.Enums;

namespace DomainModel.Models;

public class BackOfficeUserPermission
{
    public int Id { get; set; }
    public PermissionType Type { get; set; }
}