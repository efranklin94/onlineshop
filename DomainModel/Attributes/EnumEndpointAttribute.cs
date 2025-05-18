﻿namespace onlineshop.Attributes;

[AttributeUsage(AttributeTargets.Enum)]
public class EnumEndpointAttribute(string route) : Attribute
{
    public string Route { get; } = route;
}