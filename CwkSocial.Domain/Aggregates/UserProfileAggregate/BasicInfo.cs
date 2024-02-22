using CwkSocial.Domain.Exceptions;
using CwkSocial.Domain.Validators.UserProfileValidators;

namespace CwkSocial.Domain.Aggregates.UserProfileAggregate;

public class BasicInfo
{
    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string EmailAddress { get; private set; } = string.Empty;

    public string? Phone { get; private set; } = string.Empty;

    public DateTime DateOfBirth { get; private set; }

    public string? CurrentCity { get; private set; } = string.Empty;

    private BasicInfo()
    {

    }

    /// <summary>
    /// Creates a new instance of the BasicInfo class, which is
    /// a part of the UserProfile aggregate.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="emailAddress"></param>
    /// <param name="phone"></param>
    /// <param name="dateOfBirth"></param>
    /// <param name="currentCity"></param>
    /// <returns>An instance of BasicInfo</returns>
    /// <exception cref="UserProfileNotValidException"></exception>
    public static BasicInfo Create(
        string firstName,
        string lastName,
        string emailAddress,
        string? phone,
        DateTime dateOfBirth,
        string? currentCity)
    {
        var validator = new BasicInfoValidator();

        var basicInfo = new BasicInfo
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            Phone = phone,
            DateOfBirth = dateOfBirth,
            CurrentCity = currentCity
        };

        var validationResult = validator.Validate(basicInfo);

        if (validationResult.IsValid) return basicInfo;

        var exception = new UserProfileNotValidException("The basic info is not valid.")
        {
            ValidationErrors = validationResult.Errors.ConvertAll(e => e.ErrorMessage)
        };

        throw exception;
    }

    // Note: We don't have any public methods to update the basic info.
    // This is because we don't want to allow the basic info to be updated directly.
    // Instead, we want to allow the user to create a new basic info object and replace the old one.
    // And this will be the responsibility of the UserProfile aggregate.
}
