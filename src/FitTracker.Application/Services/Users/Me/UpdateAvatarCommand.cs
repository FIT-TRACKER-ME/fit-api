using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Users.Me
{
    public sealed record UpdateAvatarCommand(Stream FileStream, string FileName, string ContentType) : ICommand<string>;
}
