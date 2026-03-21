using MediatR;

namespace FitTracker.Application.Services.Workouts.GetExpirationAlerts;

public class GetWorkoutExpirationAlertsQuery : IRequest<IEnumerable<WorkoutExpirationAlertResponse>>
{
}

public class WorkoutExpirationAlertResponse
{
    public Guid WorkoutId { get; set; }
    public string WorkoutName { get; set; } = default!;
    public string StudentName { get; set; } = default!;
    public DateTime ExpirationDate { get; set; }
    public int DaysRemaining { get; set; }
}
