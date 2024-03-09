using CwkSocial.Domain.Common.Errors;

namespace CwkSocial.Domain.Aggregates.UserProfileAggregate;

public class UserProfile
{
    public Guid UserProfileId { get; private set; }

    public string? IdentityId { get; private set; } // Foreign key to IdentityUser

    public BasicInfo? BasicInfo { get; private set; }

    public DateTime CreatedDate { get; private set; }

    public DateTime LastModified { get; private set; }

    private UserProfile()
    {
        
    }

    public static UserProfile Create(string identityId, BasicInfo basicInfo)
    {
        // TODO: Add validation, error handling strategies, error notifications.

        return new UserProfile
        {
            IdentityId = identityId,
            BasicInfo = basicInfo,
            UserProfileId = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    // public methods
    public void UpdateBasicInfo(BasicInfo basicInfo)
    {
        BasicInfo = basicInfo;
        LastModified = DateTime.UtcNow;
    }
}
