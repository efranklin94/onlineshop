﻿namespace DomainModel.Models;

public class BackOfficeUserRole
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<BackOfficeUserPermission> Permissions { get; set; } = [];
}
