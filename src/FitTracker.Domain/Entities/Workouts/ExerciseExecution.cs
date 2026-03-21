using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Entities.Workouts
{
    public sealed class ExerciseExecution
    {
        public ExerciseExecution(Guid id, Guid workoutExecutionId, Guid exerciseId, string weightUsed, string repsDone)
        {
            Id = id;
            WorkoutExecutionId = workoutExecutionId;
            ExerciseId = exerciseId;
            WeightUsed = weightUsed;
            RepsDone = repsDone;
            CreatedAt = DateTime.UtcNow;
        }

        private ExerciseExecution()
        {
            WeightUsed = string.Empty;
            RepsDone = string.Empty;
        }

        public Guid Id { get; private set; }
        public Guid WorkoutExecutionId { get; private set; }
        public Guid ExerciseId { get; private set; }
        public string WeightUsed { get; private set; }
        public string RepsDone { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}
