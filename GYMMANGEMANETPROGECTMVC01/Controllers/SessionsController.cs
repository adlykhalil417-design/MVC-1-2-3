using GYMMANGEMANETPROGECTMVC01.BLL.Common;
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
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrinersForDropdownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoryForDropdownAsync(), "Id", "Name");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel vm)
        {
            if (!ModelState.IsValid) return View( vm);

            var result = await _sessionService.CreateSessionAsync(vm);


            if (result.sucess)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
           
             
                TempData["ErrorMessage"] = result.error;
                await PopulateDropdownListAsync();

            return View(vm);



        }

        [HttpGet]
        public async Task<IActionResult> Detalis(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if (session.success)
            {
                return View(session.value);
            }
            else
            {
                TempData["ErrorMessage"] = session.error;
                return RedirectToAction(nameof(Index));
            }
           
        }
        #endregion
        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (result.success)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrinersForDropdownAsync(), "Id", "Name");
                return View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrinersForDropdownAsync(), "Id", "Name");
                return View(model);
            }

            var result = await _sessionService.UpdateSessionAsync(id, model, ct);

            if (result.sucess)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrinersForDropdownAsync(), "Id", "Name");
                return View(model);
            }
        }
        #endregion
        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id);
            if (result.success)
            {
                return View();
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var res = await _sessionService.RemoveSessionAsync(id, ct);
            TempData[res.sucess ? "SuccessMessage" : "ErrorMessage"] = res.sucess ? "Session Deleted Successfully" : res.error;
            return RedirectToAction(nameof(Index));
        }

        #endregion


    }
}
