using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Users.Invite
{
    public record InviteStudentCommand(
        string Email,
        string Name,
        string Phone,
        Guid? AnamnesisFormId = null) : ICommand<InviteStudentResponse>;
}
