using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Anamnesis.GetFormById;

namespace FitTracker.Application.Services.Anamnesis.Create;

public class CreateAnamnesisFormCommand : ICommand<AnamnesisFormResponse>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string SchemaJson { get; set; } = default!;
}
