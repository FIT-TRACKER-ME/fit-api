using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.Update
{
    internal sealed class UpdateWorkoutCommandHandler : ICommandHandler<UpdateWorkoutCommand>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public UpdateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _workoutRepository = workoutRepository;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<Result> Handle(UpdateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);

            if (workout is null)
            {
                return Result.Failure(new Error("Workout.NotFound", "Treino não encontrado"));
            }

            if (workout.PersonalId.Value != _userContext.UserId)
            {
                return Result.Failure(new Error("Unauthorized", "Você não tem permissão para alterar este treino."));
            }

            var expirationDate = request.ExpirationDate.HasValue 
                ? DateTime.SpecifyKind(request.ExpirationDate.Value, DateTimeKind.Utc) 
                : (DateTime?)null;

            workout.Update(
                request.Name,
                request.Description,
                request.DurationWeeks,
                request.FrequencyDaysPerWeek,
                expirationDate);

            // Clear and Re-add WorkoutDays (Simple approach for this use case)
            workout.WorkoutDays.Clear();

            foreach (var dayRequest in request.WorkoutDays)
            {
                var workoutDay = new WorkoutDay(Guid.NewGuid(), dayRequest.Name, workout.Id);
                
                foreach (var exRequest in dayRequest.Exercises)
                {
                    workoutDay.Exercises.Add(new Exercise(
                        Guid.NewGuid(),
                        exRequest.Name,
                        exRequest.Sets,
                        exRequest.Reps,
                        exRequest.Weight,
                        exRequest.VideoUrl,
                        workoutDay.Id,
                        exRequest.RestPeriod));
                }

                workout.WorkoutDays.Add(workoutDay);
            }

            _workoutRepository.Update(workout);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
