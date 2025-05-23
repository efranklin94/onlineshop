namespace DomainModel.Models;

public class UserOption
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public UserOption(string description)
    {
        Description = description;
    }

    public static UserOption Create(string description)
    {
        return new UserOption(description);
    }
}
