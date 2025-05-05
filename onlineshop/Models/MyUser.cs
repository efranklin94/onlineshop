namespace onlineshop.Models;

public class MyUser
{
    public int Id { get; set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Email { get; set; }

    private MyUser(string firstName, string lastName, string phoneNumber, string email)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetPhoneNumber(phoneNumber);
        SetIsActive(true);
        SetEmail(email);
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

    private void SetEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        Email = email;
    }
}