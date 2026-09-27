using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces
{
    public interface IPlanRepository:IGenericRepository<Plan>
    {
        Task<IEnumerable<Plan>> GetPlansWithMembers();
    }
}
