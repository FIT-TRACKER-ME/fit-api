using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Entities.Anamnesis
{
    public class AnamnesisForm
    {
        public AnamnesisForm(Guid id, string title, string description, string schemaJson, UserId personalId)
        {
            Id = id;
            Title = title;
            Description = description;
            SchemaJson = schemaJson;
            PersonalId = personalId;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        private AnamnesisForm()
        {
            Title = string.Empty;
            Description = string.Empty;
            SchemaJson = string.Empty;
            PersonalId = null!;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string SchemaJson { get; private set; } // JSON defining the fields
        public UserId PersonalId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public void Deactivate() => IsActive = false;
    }
}
