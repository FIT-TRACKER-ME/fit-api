using FitTracker.Domain.Entities.Anamnesis;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infra.Repositories
{
    public class AnamnesisRepository : IAnamnesisRepository
    {
        private readonly DatabaseContext _context;

        public AnamnesisRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<AnamnesisForm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.AnamnesisForms
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<AnamnesisForm>> GetByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken = default)
        {
            return await _context.AnamnesisForms
                .Where(x => x.PersonalId == personalId && x.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AnamnesisResponse>> GetResponsesByStudentIdAsync(UserId studentId, CancellationToken cancellationToken = default)
        {
            return await _context.AnamnesisResponses
                .Where(x => x.StudentId == studentId)
                .ToListAsync(cancellationToken);
        }

        public void Add(AnamnesisForm anamnesisForm)
        {
            _context.AnamnesisForms.Add(anamnesisForm);
        }

        public void Update(AnamnesisForm anamnesisForm)
        {
            _context.AnamnesisForms.Update(anamnesisForm);
        }
    }
}
