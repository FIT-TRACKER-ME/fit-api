using Microsoft.EntityFrameworkCore;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Entities.RefreshTokens;
using FitTracker.Domain.Entities.Anamnesis;

namespace FitTracker.Infra.Context
{
    public sealed class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        //entities
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Workout> Workouts { get; set; } = default!;
        public DbSet<WorkoutDay> WorkoutDays { get; set; } = default!;
        public DbSet<Exercise> Exercises { get; set; } = default!;
        public DbSet<WorkoutExecution> WorkoutExecutions { get; set; } = default!;
        public DbSet<ExerciseExecution> ExerciseExecutions { get; set; } = default!;
        public DbSet<AnamnesisForm> AnamnesisForms { get; set; } = default!;
        public DbSet<AnamnesisResponse> AnamnesisResponses { get; set; } = default!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

}
