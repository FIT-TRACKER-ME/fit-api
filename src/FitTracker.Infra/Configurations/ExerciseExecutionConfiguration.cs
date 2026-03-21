using FitTracker.Domain.Entities.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infra.Configurations
{
    internal class ExerciseExecutionConfiguration : IEntityTypeConfiguration<ExerciseExecution>
    {
        public void Configure(EntityTypeBuilder<ExerciseExecution> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.WeightUsed).HasMaxLength(50).IsRequired();
            builder.Property(x => x.RepsDone).HasMaxLength(50).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne<WorkoutExecution>()
                .WithMany()
                .HasForeignKey(x => x.WorkoutExecutionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Exercise>()
                .WithMany()
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("ExerciseExecutions");
        }
    }
}
