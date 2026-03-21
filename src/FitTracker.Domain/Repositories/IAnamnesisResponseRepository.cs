using FitTracker.Domain.Entities.Anamnesis;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Repositories
{
    public interface IAnamnesisResponseRepository
    {
        Task<AnamnesisResponse?> GetByStudentIdAsync(UserId studentId, CancellationToken cancellationToken = default);
        void Add(AnamnesisResponse anamnesisResponse);
    }
}
