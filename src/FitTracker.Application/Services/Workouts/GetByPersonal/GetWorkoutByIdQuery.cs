using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Workouts.GetByStudent;

namespace FitTracker.Application.Services.Workouts.GetByPersonal
{
    public record GetWorkoutByIdQuery(Guid WorkoutId) : IQuery<WorkoutResponse>;
}
