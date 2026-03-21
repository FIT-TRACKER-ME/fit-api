using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Workouts.Create
{
    public class CreateWorkoutCommand : ICommand<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationWeeks { get; set; }
        public int FrequencyDaysPerWeek { get; set; }
        public Guid StudentId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<WorkoutDayRequest> WorkoutDays { get; set; } = new List<WorkoutDayRequest>();
    }public record WorkoutDayRequest(string Name, List<ExerciseRequest> Exercises);

    public record ExerciseRequest(string Name, string Sets, string Reps, string Weight, string VideoUrl, int RestPeriod);
}
