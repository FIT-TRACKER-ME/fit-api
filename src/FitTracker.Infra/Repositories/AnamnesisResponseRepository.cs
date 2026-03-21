using FitTracker.Domain.Entities.Anamnesis;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infra.Repositories
{
    public class AnamnesisResponseRepository : IAnamnesisResponseRepository
    {
        private readonly DatabaseContext _context;

        public AnamnesisResponseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<AnamnesisResponse?> GetByStudentIdAsync(UserId studentId, CancellationToken cancellationToken = default)
        {
            return await _context.AnamnesisResponses
                .FirstOrDefaultAsync(x => x.StudentId == studentId, cancellationToken);
        }

        public void Add(AnamnesisResponse anamnesisResponse)
        {
            _context.AnamnesisResponses.Add(anamnesisResponse);
        }
    }
}
