using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Entities.Anamnesis
{
    public class AnamnesisResponse
    {
        public AnamnesisResponse(Guid id, Guid anamnesisFormId, UserId studentId, string responsesJson)
        {
            Id = id;
            AnamnesisFormId = anamnesisFormId;
            StudentId = studentId;
            ResponsesJson = responsesJson;
            CreatedAt = DateTime.UtcNow;
        }

        private AnamnesisResponse()
        {
            StudentId = null!;
            ResponsesJson = string.Empty;
        }

        public Guid Id { get; private set; }
        public Guid AnamnesisFormId { get; private set; }
        public UserId StudentId { get; private set; }
        public string ResponsesJson { get; private set; } // JSON with user answers
        public DateTime CreatedAt { get; private set; }
    }
}
