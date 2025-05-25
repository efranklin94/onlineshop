﻿namespace onlineshop;

public class Settings
{
    public required TrackingCodeSettings TrackingCode { get; set; }
    public required JWTSettings JWT { get; set; }
}

public class TrackingCodeSettings
{
    public required string BaseURL { get; set; }
    public required string GetURL { get; set; }
    public required string Prefix { get; set; }
}
public class JWTSettings
{
    public required string Key { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int ExpireInMinutes { get; set; }
}