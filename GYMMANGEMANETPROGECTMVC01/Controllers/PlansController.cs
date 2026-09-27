using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GYMMANGEMANETPROGECTMVC01.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepository;

        public PlansController(IGenericRepository<Plan> planRepository)//By DI
        {
            _planRepository = planRepository;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);


            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);
            if (plan is null) return RedirectToAction(nameof(Index));

            return View(plan);
        }
    }
}
