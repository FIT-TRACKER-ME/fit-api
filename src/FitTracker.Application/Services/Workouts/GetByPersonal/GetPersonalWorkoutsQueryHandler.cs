using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Workouts.GetByStudent;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.GetByPersonal
{
    internal sealed class GetPersonalWorkoutsQueryHandler : IQueryHandler<GetPersonalWorkoutsQuery, List<WorkoutResponse>>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUserContext _userContext;

        public GetPersonalWorkoutsQueryHandler(IWorkoutRepository workoutRepository, IUserContext userContext)
        {
            _workoutRepository = workoutRepository;
            _userContext = userContext;
        }

        public async Task<Result<List<WorkoutResponse>>> Handle(GetPersonalWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetByPersonalIdAsync(new UserId(_userContext.UserId), cancellationToken);

            var response = workouts.Select(WorkoutResponse.FromDomain).ToList();

            return response;
        }
    }
}
