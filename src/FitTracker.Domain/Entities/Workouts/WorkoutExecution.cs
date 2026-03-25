using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Entities.Workouts
{
    public sealed class WorkoutExecution
    {
        public WorkoutExecution(Guid id, Guid workoutId, UserId studentId, int? rating = null, string? feedback = null)
        {
            Id = id;
            WorkoutId = workoutId;
            StudentId = studentId;
            CompletedAt = DateTime.UtcNow;
            Rating = rating;
            Feedback = feedback;
        }

        private WorkoutExecution()
        {
            StudentId = null!;
        }

        public Guid Id { get; private set; }
        public Guid WorkoutId { get; private set; }
        public UserId StudentId { get; private set; }
        public DateTime CompletedAt { get; private set; }
        public int? Rating { get; private set; }
        public string? Feedback { get; private set; }

        public void UpdateFeedback(int? rating, string? feedback)
        {
            Rating = rating;
            Feedback = feedback;
        }
    }
}
