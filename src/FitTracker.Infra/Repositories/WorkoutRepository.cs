using Microsoft.EntityFrameworkCore;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Repositories;
using FitTracker.Infra.Context;

namespace FitTracker.Infra.Repositories
{
    public sealed class WorkoutRepository : IWorkoutRepository
    {
        private readonly DatabaseContext _dbContext;

        public WorkoutRepository(DatabaseContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await _dbContext
                .Set<Workout>()
                .Include(w => w.Student)
                .Include(w => w.WorkoutDays)
                .ThenInclude(wd => wd.Exercises)
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        public async Task<IEnumerable<Workout>> GetByStudentIdAsync(UserId studentId, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<Workout>()
                .Include(x => x.Student)
                .Include(x => x.WorkoutDays)
                    .ThenInclude(xd => xd.Exercises)
                .Where(x => x.StudentId == studentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Workout>> GetByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<Workout>()
                .Include(x => x.Student)
                .Include(x => x.WorkoutDays)
                    .ThenInclude(xd => xd.Exercises)
                .Where(x => x.PersonalId == personalId)
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Workout>> GetExpirationAlertsAsync(UserId personalId, DateTime thresholdDate, CancellationToken cancellationToken) =>
            await _dbContext
                .Set<Workout>()
                .Include(w => w.Student)
                .Where(w => w.PersonalId == personalId && 
                            w.ExpirationDate <= thresholdDate && 
                            w.ExpirationDate >= DateTime.UtcNow)
                .ToListAsync(cancellationToken);

        public async Task<int> CountByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken) =>
            await _dbContext
                .Set<Workout>()
                .CountAsync(w => w.PersonalId == personalId, cancellationToken);

        public async Task<int> CountAllAsync(CancellationToken cancellationToken) =>
            await _dbContext
                .Set<Workout>()
                .CountAsync(cancellationToken);

        public void Add(Workout workout) =>
            _dbContext.Set<Workout>().Add(workout);

        public void Update(Workout workout) =>
            _dbContext.Set<Workout>().Update(workout);

        public void Remove(Workout workout) =>
            _dbContext.Set<Workout>().Remove(workout);
    }
}
