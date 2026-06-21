using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct)
        {
            var sessions = dbContext.sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);
            return await sessions.ToListAsync(ct);
        }

        public Task<int> GetCountOfBookedSlotAsync(int SessionId, CancellationToken ct)
        {
           return dbContext.Bookings.AsNoTracking().CountAsync(b =>b.SessionId == SessionId);
        }

        public async Task<Session> GetSessionsByIdWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct)
        {
            var Session = dbContext.sessions. Include(s => s.Trainer)
                .Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == SessionId);
            return await Session;
        }
    }
}
