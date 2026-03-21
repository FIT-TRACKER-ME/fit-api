using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Workouts;

namespace FitTracker.Application.Services.Workouts.GetByStudent
{
    public record GetStudentWorkoutsQuery(Guid StudentId) : IQuery<List<WorkoutResponse>>;

    public record WorkoutResponse(
        Guid Id, 
        string Name, 
        string Description, 
        int DurationWeeks, 
        int FrequencyDaysPerWeek, 
        DateTime CreatedAt,
        List<WorkoutDayResponse> WorkoutDays,
        Guid StudentId,
        string? StudentName = null,
        DateTime? ExpirationDate = null)
    {
        public static WorkoutResponse FromDomain(Workout workout) =>
            new(
                workout.Id,
                workout.Name,
                workout.Description,
                workout.DurationWeeks,
                workout.FrequencyDaysPerWeek,
                workout.CreatedAt,
                workout.WorkoutDays.Select(WorkoutDayResponse.FromDomain).ToList(),
                workout.StudentId.Value,
                workout.Student?.Name,
                workout.ExpirationDate
            );
    }

    public record WorkoutDayResponse(Guid Id, string Name, List<ExerciseResponse> Exercises)
    {
        public static WorkoutDayResponse FromDomain(WorkoutDay day) =>
            new(
                day.Id,
                day.Name,
                day.Exercises.Select(ExerciseResponse.FromDomain).ToList()
            );
    }

    public record ExerciseResponse(Guid Id, string Name, string Sets, string Reps, string Weight, string VideoUrl)
    {
        public static ExerciseResponse FromDomain(Exercise exercise) =>
            new(
                exercise.Id,
                exercise.Name,
                exercise.Sets,
                exercise.Reps,
                exercise.Weight,
                exercise.VideoUrl
            );
    }
}
