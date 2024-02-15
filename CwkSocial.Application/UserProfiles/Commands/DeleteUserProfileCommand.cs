
using MediatR;

namespace CwkSocial.Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest<Unit>
{
    public Guid UserProfileId { get; set; }
}
