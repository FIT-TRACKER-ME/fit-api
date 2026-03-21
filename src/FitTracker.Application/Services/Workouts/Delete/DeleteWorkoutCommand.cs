using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Workouts.Delete
{
    public record DeleteWorkoutCommand(Guid WorkoutId) : ICommand;
}
