
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace CwkSocial.DataAccess.Models;

public class ApplicationUser : IdentityUser
{
    //public string FirstName { get; private set; } = string.Empty;
    //public string LastName { get; private set; } = string.Empty;
    //public string? CurrentCity { get; init; }
    //public DateTime DateOfBirth { get; private set; }
    public DateTime CreatedDate { get; private set; }

    // Factory method to create a new user
    public static ErrorOr<ApplicationUser> Create(
        string userName, 
        string email, 
        string firstName, 
        string lastName, 
        DateTime dateOfBirth, 
        string? currentCity)
    {
        // Validate the input
        if (string.IsNullOrWhiteSpace(userName))
            return Errors.Identity.InvalidUserName;

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            CreatedDate = DateTime.UtcNow,
            //FirstName = firstName,
            //LastName = lastName,
            //DateOfBirth = dateOfBirth,
            //CurrentCity = currentCity,
        };

        return user;
    }
}
