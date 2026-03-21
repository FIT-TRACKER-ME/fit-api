using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.Delete
{
    internal sealed class DeleteWorkoutCommandHandler : ICommandHandler<DeleteWorkoutCommand>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public DeleteWorkoutCommandHandler(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _workoutRepository = workoutRepository;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);

            if (workout is null)
            {
                return Result.Failure(new Error("Workout.NotFound", "Treino não encontrado"));
            }

            if (workout.PersonalId.Value != _userContext.UserId)
            {
                return Result.Failure(new Error("Unauthorized", "Você não tem permissão para deletar este treino."));
            }

            _workoutRepository.Remove(workout);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
