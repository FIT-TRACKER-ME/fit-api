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

        public async Task<WorkoutResponse> SignUrlsAsync(FitTracker.Application.Abstractions.IBlobStorageService storageService)
        {
            var signedDays = new List<WorkoutDayResponse>();
            foreach (var day in WorkoutDays)
            {
                signedDays.Add(await day.SignUrlsAsync(storageService));
            }
            return this with { WorkoutDays = signedDays };
        }
    }

    public record WorkoutDayResponse(Guid Id, string Name, List<ExerciseResponse> Exercises)
    {
        public static WorkoutDayResponse FromDomain(WorkoutDay day) =>
            new(
                day.Id,
                day.Name,
                day.Exercises.Select(ExerciseResponse.FromDomain).ToList()
            );

        public async Task<WorkoutDayResponse> SignUrlsAsync(FitTracker.Application.Abstractions.IBlobStorageService storageService)
        {
            var signedExercises = new List<ExerciseResponse>();
            foreach (var ex in Exercises)
            {
                signedExercises.Add(await ex.SignUrlsAsync(storageService));
            }
            return this with { Exercises = signedExercises };
        }
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

        public async Task<ExerciseResponse> SignUrlsAsync(FitTracker.Application.Abstractions.IBlobStorageService storageService)
        {
            if (string.IsNullOrEmpty(VideoUrl)) return this;
            var signedUrl = await storageService.GetDownloadUrlAsync(VideoUrl);
            return this with { VideoUrl = signedUrl };
        }
    }
}
