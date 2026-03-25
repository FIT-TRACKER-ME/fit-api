using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.Execute
{
    internal sealed class AddFeedbackCommandHandler : ICommandHandler<AddFeedbackCommand>
    {
        private readonly IWorkoutExecutionRepository _workoutExecutionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddFeedbackCommandHandler(
            IWorkoutExecutionRepository workoutExecutionRepository,
            IUnitOfWork unitOfWork)
        {
            _workoutExecutionRepository = workoutExecutionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
        {
            var execution = await _workoutExecutionRepository.GetByIdAsync(request.ExecutionId, cancellationToken);

            if (execution == null)
            {
                return Result.Failure(new Error("WorkoutExecution.NotFound", "Sessão de treino não encontrada."));
            }

            execution.UpdateFeedback(request.Rating, request.Feedback);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
