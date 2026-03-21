using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Anamnesis.Deactivate
{
    public record DeactivateAnamnesisFormCommand(Guid FormId) : ICommand;
}
