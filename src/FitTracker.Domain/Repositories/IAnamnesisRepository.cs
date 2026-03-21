using FitTracker.Domain.Entities.Anamnesis;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Repositories
{
    public interface IAnamnesisRepository
    {
        Task<AnamnesisForm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<AnamnesisForm>> GetByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AnamnesisResponse>> GetResponsesByStudentIdAsync(UserId studentId, CancellationToken cancellationToken = default);
        void Add(AnamnesisForm anamnesisForm);
        void Update(AnamnesisForm anamnesisForm);
    }
}
