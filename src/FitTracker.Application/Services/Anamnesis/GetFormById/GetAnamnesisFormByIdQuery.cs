using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Anamnesis.GetFormById;

public class GetAnamnesisFormByIdQuery : IQuery<AnamnesisFormResponse>
{
    public Guid Id { get; set; }
}

public class AnamnesisFormResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string SchemaJson { get; set; } = default!;
}
