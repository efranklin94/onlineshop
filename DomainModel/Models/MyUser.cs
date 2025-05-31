using DomainModel.Models;

namespace onlineshop.Models;

public class MyUser : TrackableEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Email { get; set; }
    public string TrackingCode { get; set; }

    public List<UserOption> userOptions { get; set; } = [];
    public List<UserTag> userTags { get; set; } = [];

    private MyUser(string firstName, string lastName, string phoneNumber, string? email)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetPhoneNumber(phoneNumber);
        SetIsActive(true);
        SetEmail(email!);
    }

    public static MyUser Create(string firstName, string lastName, string phoneNumber, string email)
    {
        return new MyUser(firstName, lastName, phoneNumber, email);
    }

    public void Update(string firstName, string lastName,string phoneNumber)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetPhoneNumber(phoneNumber);
    }
    private void SetFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            throw new ArgumentNullException(nameof(firstName));
        }
        FirstName = firstName; 
    }
    private void SetLastName(string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
        {
            throw new ArgumentNullException(nameof(lastName));
        }
        LastName = lastName;
    }
    private void SetPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            throw new ArgumentNullException(nameof(phoneNumber));
        }
        PhoneNumber = phoneNumber;
    }
    public void SetIsActive(bool isActive)
    { 
        IsActive = isActive; 
    }

    private void SetEmail(string? email)
    {
        Email = email;
    }
    public void SetTrackingCode(string trackingCode)
    {
        TrackingCode = trackingCode;
    }
    public void AddOption(string description)
    {
        var option = UserOption.Create(description);
        userOptions.Add(option);
    }

    public void RemoveOption(Guid optionId)
    {
        var option = userOptions.FirstOrDefault(x => x.Id == optionId);
        if (option != null)
        {
            userOptions.Remove(option);
        }
    }

    public void AddTag(string title, int priority)
    {
        var tag = UserTag.Create(title, priority);
        userTags.Add(tag);
    }

    public void RemoveTag(string title, int priority)
    {
        var tag = UserTag.Create(title, priority);
        userTags.Remove(tag);
    }
}