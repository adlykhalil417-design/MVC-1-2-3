using GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GYMMANGEMANETPROGECTMVC01.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;  
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();


            return View(members);
        }

        //Get Member Details


        #region Create

        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel member)
        {
            if (!ModelState.IsValid) return View(nameof(Create), member);

            var result = await _memberService.CreateMemberAsync(member);


            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed To  Create Member  ";
            }

            return RedirectToAction(nameof(Index));



        }
        #endregion


        //Get  UpdateMember
        //Post UpdateMember
    }
}
