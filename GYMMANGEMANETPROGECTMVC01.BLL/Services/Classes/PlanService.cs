using AutoMapper;
using GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.PlanViweModel;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.BLL.Services.Classes
{
    public class PlanService:IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<MemberShip> _membershipRepository;

        public PlanService(IGenericRepository<Plan> planRepository, IGenericRepository<MemberShip> membershipRepository)
        {
            _planRepository = planRepository;
            _membershipRepository = membershipRepository;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return plans.Select(p => new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive,
                Price = p.Price,
            });
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct: ct);
            if (plan is null) return null;

            return new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive
            };
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct: ct);
            if (plan is null || !plan.IsActive ||
                await HasActiveMembershipsAsync(id, ct))
                return null;

            //Active Plan

            return new UpdatePlanViewModel()
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };
        }

        public async Task<bool> ToggleActivationStatusAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct: ct);

            if (plan is null || await HasActiveMembershipsAsync(id, ct)) return false;
            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAt = DateTime.Now;
            return await _planRepository.UpdateAsync(plan, ct) > 0;
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel planViewModel, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct: ct);
            if (plan is null || await HasActiveMembershipsAsync(id, ct)) return false;

            plan.Description = planViewModel.Description;
            plan.Price = planViewModel.Price;
            plan.DurationDays = planViewModel.DurationDays;
            plan.UpdatedAt = DateTime.Now;
            return await _planRepository.UpdateAsync(plan, ct) > 0;
        }

        #region Helper Methods

        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct = default)
        {
            return await _membershipRepository.AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
        }

        #endregion
    }
}

