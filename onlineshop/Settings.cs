namespace onlineshop;

public class Settings
{
    public required TrackingCodeSettings TrackingCode { get; set; }
}

public class TrackingCodeSettings
{
    public required string BaseURL { get; set; }
    public required string GetURL { get; set; }
    public required string Prefix { get; set; }
}
