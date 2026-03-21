using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Workouts.GetByStudent;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.GetByPersonal
{
    internal sealed class GetWorkoutByIdQueryHandler : IQueryHandler<GetWorkoutByIdQuery, WorkoutResponse>
    {
        private readonly IWorkoutRepository _workoutRepository;

        public GetWorkoutByIdQueryHandler(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<Result<WorkoutResponse>> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
        {
            var w = await _workoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);

            if (w is null)
            {
                return Result.Failure<WorkoutResponse>(new Error("Workout.NotFound", "Treino não encontrado"));
            }

            var response = WorkoutResponse.FromDomain(w);

            return response;
        }
    }
}
