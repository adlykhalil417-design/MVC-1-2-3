using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using GYMMANGEMANETPROGECTMVC01.DAL.DATA.DbContexts;

namespace GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Classes
{
    public class PlanRepository : GenericRepository<Plan> , IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext):base(dbContext)
        {
            _dbContext=dbContext;
        }


    

        public async Task<IEnumerable<Plan>> GetPlansWithMembers()
        {
            return await _dbContext.Plans.Include(p => p.Memberships).ToListAsync();
        }
    }
}
