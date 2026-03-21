using MediatR;
using FitTracker.Application.Abstractions;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Application.Services.Workouts.GetExpirationAlerts;

public class GetWorkoutExpirationAlertsQueryHandler : IRequestHandler<GetWorkoutExpirationAlertsQuery, IEnumerable<WorkoutExpirationAlertResponse>>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IUserContext _userContext;

    public GetWorkoutExpirationAlertsQueryHandler(IWorkoutRepository workoutRepository, IUserContext userContext)
    {
        _workoutRepository = workoutRepository;
        _userContext = userContext;
    }

    public async Task<IEnumerable<WorkoutExpirationAlertResponse>> Handle(GetWorkoutExpirationAlertsQuery request, CancellationToken cancellationToken)
    {
        var sevenDaysFromNow = DateTime.UtcNow.AddDays(7);
        
        var workouts = await _workoutRepository.GetExpirationAlertsAsync(new UserId(_userContext.UserId), sevenDaysFromNow, cancellationToken);

        return workouts.Select(w => new WorkoutExpirationAlertResponse
        {
            WorkoutId = w.Id,
            WorkoutName = w.Name,
            StudentName = w.Student.Name,
            ExpirationDate = w.ExpirationDate!.Value,
            DaysRemaining = (w.ExpirationDate.Value - DateTime.UtcNow).Days
        });
    }
}
