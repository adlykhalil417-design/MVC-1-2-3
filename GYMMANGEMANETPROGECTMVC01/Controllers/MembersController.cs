using GYMMANGEMANETPROGECTMVC01.BLL.Services.Attachment;
using GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace GYMMANGEMANETPROGECTMVC01.PL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;

        public MembersController(IMemberService memberService,IAttachmentService attachmentService)
        {
            _memberService = memberService;
            _attachmentService = attachmentService;
        }


   
        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();


            return View(members);
        }

        //Get Member Details



        #region Picture : Get Member Picture

        [HttpGet]
        public async Task<IActionResult> Picture(int id)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id);

            if (member is null || string.IsNullOrEmpty(member.Photo)) return NotFound();


            var result = _attachmentService.GetFile(member.Photo, "MembersPhotos");
            return File(result.Value.stream, result.Value.contentType);

        }

        #endregion

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

        public async Task<IActionResult>MemberDetails(int id,CancellationToken ct)
        {
            var member =await  _memberService.GetMemberDetailsByIdAsync(id,ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not Found";
                return RedirectToAction(nameof(Index));
            }
              return View(member);
        }
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthrecord = await _memberService.GetHealthRecordAync(id,ct);
            if (healthrecord == null)
            {
                TempData["ErrorMessage"] = "Health Record not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthrecord);
        }
        #region editmember 

        //Get  UpdateMember
        [HttpGet]
        public async Task<IActionResult> EditMember(int Id,CancellationToken ct)
        {
            var member=await _memberService.GetMemberToUpdateASync(Id,ct);
            if (member == null) {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index)); 
            }
            return View(member);

        }
        //Post UpdateMember
        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel memberVM, CancellationToken ct = default)
        {
            if(!ModelState.IsValid)return View(memberVM);
            var res=await _memberService.UpdateMemberDetailsAsync(id,memberVM,ct);
            if (res)
            {
                TempData["SuccessMessage"] = "Member  Update Successfully";
              
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed To  Update Member  ";


            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region delete 

        
        [HttpGet]
        public async Task<IActionResult> Delete(int Id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(Id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id, CancellationToken ct )
        {
            

            var res = await _memberService.RemoveMemberAsync(id,  ct);
            if (res)
            {
                TempData["SuccessMessage"] = "Member  Deleted Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = $"Failed To  Deleted Member  ";


            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
