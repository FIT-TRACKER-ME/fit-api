using FitTracker.Domain.Entities.Anamnesis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infra.Configurations
{
    internal class AnamnesisFormConfiguration : IEntityTypeConfiguration<AnamnesisForm>
    {
        public void Configure(EntityTypeBuilder<AnamnesisForm> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(512);
            builder.Property(x => x.SchemaJson).IsRequired(); // Store as text/json
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.PersonalId).HasConversion(
                id => id.Value,
                value => new FitTracker.Domain.Entities.Users.UserId(value));

            builder.ToTable("AnamnesisForms");
        }
    }
}
