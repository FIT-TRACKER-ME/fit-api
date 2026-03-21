using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Workouts.Create;

namespace FitTracker.Application.Services.Workouts.Update
{
    public record UpdateWorkoutCommand(
        Guid WorkoutId,
        string Name,
        string Description,
        int DurationWeeks,
        int FrequencyDaysPerWeek,
        DateTime? ExpirationDate,
        List<WorkoutDayRequest> WorkoutDays) : ICommand;
}
