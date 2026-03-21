using FitTracker.Domain.Entities.Anamnesis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infra.Configurations
{
    internal class AnamnesisResponseConfiguration : IEntityTypeConfiguration<AnamnesisResponse>
    {
        public void Configure(EntityTypeBuilder<AnamnesisResponse> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ResponsesJson).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.StudentId).HasConversion(
                id => id.Value,
                value => new FitTracker.Domain.Entities.Users.UserId(value));

            builder.HasOne<AnamnesisForm>()
                .WithMany()
                .HasForeignKey(x => x.AnamnesisFormId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("AnamnesisResponses");
        }
    }
}
