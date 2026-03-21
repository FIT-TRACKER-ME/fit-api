using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.GetByStudent
{
    internal sealed class GetStudentWorkoutsQueryHandler : IQueryHandler<GetStudentWorkoutsQuery, List<WorkoutResponse>>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public GetStudentWorkoutsQueryHandler(IWorkoutRepository workoutRepository, IUserContext userContext, IUserRepository userRepository)
        {
            _workoutRepository = workoutRepository;
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<Result<List<WorkoutResponse>>> Handle(GetStudentWorkoutsQuery request, CancellationToken cancellationToken)
        {
            if (_userContext.Role == FitTracker.Domain.Enums.UserRole.Student)
            {
                if (_userContext.UserId != request.StudentId)
                {
                    return Result.Failure<List<WorkoutResponse>>(new Error("Unauthorized", "Você não tem permissão para acessar os treinos deste aluno."));
                }
            }
            else if (_userContext.Role == FitTracker.Domain.Enums.UserRole.Personal)
            {
                var students = await _userRepository.GetStudentsByPersonalIdAsync(new UserId(_userContext.UserId), cancellationToken);
                if (!students.Any(s => s.Id.Value == request.StudentId))
                {
                    return Result.Failure<List<WorkoutResponse>>(new Error("Unauthorized", "Você não tem permissão para acessar os treinos deste aluno."));
                }
            }

            var workouts = await _workoutRepository.GetByStudentIdAsync(new UserId(request.StudentId), cancellationToken);

            var response = workouts.Select(WorkoutResponse.FromDomain).ToList();

            return response;
        }
    }
}
