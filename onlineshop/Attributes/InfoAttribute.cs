namespace onlineshop.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class InfoAttribute(string key, object value) : Attribute
{
    public string Key { get; set; } = key;
    public object Value { get; set; } = value;
}
