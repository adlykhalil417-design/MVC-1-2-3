using GYMMANGEMANETPROGECTMVC01.BLL.Services.Classes;
using GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMMANGEMANETPROGECTMVC01.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public  async Task<IActionResult> Index()
        {
            var sessions=await _sessionService.GetAllSessionsAsync();
            return View(sessions);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownListAsync();
            return View();
        }

        private async Task PopulateDropdownListAsync()
        {
            ViewBag.trainers = new SelectList(await _sessionService.GetTrinersForDropdownAsync(), "Id", "Name");
            ViewBag.categorys = new SelectList(await _sessionService.GetCategoryForDropdownAsync(), "Id", "Name");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel vm)
        {
            if (!ModelState.IsValid) return View( vm);

            var result = await _sessionService.CreateSessionAsync(vm);


            if (result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                await PopulateDropdownListAsync();
                TempData["ErrorMessage"] = $"Failed To  Create Session  ";
            }

            return View(vm);



        }
        #endregion
    
        

        
    }
}
