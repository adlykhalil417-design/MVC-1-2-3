using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces
{
    public interface ISessionRepository:IGenericRepository<Session>
    {
        Task<IEnumerable<Session>>GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct= default);
        Task<int>GetcountofBookedSlotsAsync(int sessionId,CancellationToken ct= default);
        Task<Session> GetSessionsByIdWithTrainerAndCategoryAsync(int sessionid,CancellationToken ct = default);
    }
}
