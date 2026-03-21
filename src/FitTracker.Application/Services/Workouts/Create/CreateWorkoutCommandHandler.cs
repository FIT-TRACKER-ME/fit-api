using System;
using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.Create
{
    internal sealed class CreateWorkoutCommandHandler : ICommandHandler<CreateWorkoutCommand, Guid>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public CreateWorkoutCommandHandler(
            IWorkoutRepository workoutRepository, 
            IUnitOfWork unitOfWork,
            IUserContext userContext,
            IUserRepository userRepository)
        {
            _workoutRepository = workoutRepository;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var students = await _userRepository.GetStudentsByPersonalIdAsync(new UserId(_userContext.UserId), cancellationToken);
            if (!students.Any(s => s.Id.Value == request.StudentId))
            {
                return Result.Failure<Guid>(new Error("Unauthorized", "Você não tem permissão para criar treinos para este aluno. O aluno não está vinculado ao seu perfil."));
            }

            var expirationDate = request.ExpirationDate.HasValue 
                ? DateTime.SpecifyKind(request.ExpirationDate.Value, DateTimeKind.Utc) 
                : (DateTime?)null;

            var workout = new Workout(
                Guid.NewGuid(),
                request.Name,
                request.Description ?? string.Empty,
                request.DurationWeeks,
                request.FrequencyDaysPerWeek,
                new UserId(request.StudentId),
                new UserId(_userContext.UserId),
                expirationDate
            );

            foreach (var dayRequest in request.WorkoutDays)
            {
                var workoutDay = new WorkoutDay(Guid.NewGuid(), dayRequest.Name, workout.Id);
                
                foreach (var exerciseRequest in dayRequest.Exercises)
                {
                    var exercise = new Exercise(
                        Guid.NewGuid(),
                        exerciseRequest.Name,
                        exerciseRequest.Sets,
                        exerciseRequest.Reps,
                        exerciseRequest.Weight,
                        exerciseRequest.VideoUrl,
                        workoutDay.Id,
                        exerciseRequest.RestPeriod
                    );
                    workoutDay.Exercises.Add(exercise);
                }
                
                workout.WorkoutDays.Add(workoutDay);
            }

            _workoutRepository.Add(workout);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return workout.Id;
        }
    }
}
