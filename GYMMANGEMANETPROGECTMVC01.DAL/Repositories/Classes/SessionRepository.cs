using GYMMANGEMANETPROGECTMVC01.DAL.DATA.DbContexts;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>,ISessionRepository
    {
        private readonly GymDbContext _dbContext;
        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default)
        {
           var query=_dbContext.Sessions.AsNoTracking().Include(s=>s.Trainer).Include(s=>s.Category);
            return await query.ToListAsync(ct);
        }

        public async Task<int> GetcountofBookedSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return await _dbContext.Bookings.CountAsync(b=> b.SessionId == sessionId);
        }

        public async  Task<Session> GetSessionsByIdWithTrainerAndCategoryAsync(int sessionid, CancellationToken ct = default)
        {
            return await  _dbContext.Sessions.AsNoTracking().Include(s=>s.Trainer  ).Include(s=>s.Category).FirstOrDefaultAsync(s=>s.Id==sessionid);
            
        }
    }
}
