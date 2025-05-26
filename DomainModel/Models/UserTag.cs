namespace DomainModel.Models;

public class UserTag
{
    public Guid Id { get; set; }      // ← new
    public string Title { get; set; } = string.Empty;
    public int Priority { get; set; }

    public UserTag(string title, int priority)
    {
        Title = title; Priority = priority; 
    }

    public static UserTag Create(string title, int priority)
    { 
        return new UserTag(title, priority);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is not UserTag tag) return false;

        return Title == tag.Title && Priority == tag.Priority;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Priority);
    }
}
