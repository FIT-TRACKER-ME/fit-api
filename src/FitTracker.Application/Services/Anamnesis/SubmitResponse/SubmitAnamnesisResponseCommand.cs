using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Anamnesis.SubmitResponse;

public class SubmitAnamnesisResponseCommand : ICommand
{
    public Guid FormId { get; set; }
    public Guid StudentId { get; set; }
    public string AnswersJson { get; set; } = default!;
}
