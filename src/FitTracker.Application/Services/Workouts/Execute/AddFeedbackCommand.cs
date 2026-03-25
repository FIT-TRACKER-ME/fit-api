using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Workouts.Execute
{
    public sealed record AddFeedbackCommand(
        Guid ExecutionId, 
        int Rating, 
        string Feedback) : ICommand;
}
