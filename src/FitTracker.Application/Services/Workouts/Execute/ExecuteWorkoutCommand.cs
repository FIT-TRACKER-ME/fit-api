using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Workouts.Execute
{
    public sealed record ExecuteWorkoutCommand(
        Guid WorkoutId, 
        int? Rating = null, 
        string? Feedback = null,
        List<ExerciseExecutionRequest>? ExerciseExecutions = null) : ICommand<Guid>;

    public sealed record ExerciseExecutionRequest(Guid ExerciseId, string WeightUsed, string RepsDone);
}
