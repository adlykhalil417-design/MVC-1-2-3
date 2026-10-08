using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.PlanViweModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);

        Task<PlanViewModel?> GetPlanByIdAsync(int id, CancellationToken ct = default);

        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default);

        Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel plan, CancellationToken ct = default);

        Task<bool> ToggleActivationStatusAsync(int id, CancellationToken ct = default);
    }
}
