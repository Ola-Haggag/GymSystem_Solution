using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices memberServices;

        public MemberController(IMemberServices memberServices) 
        {
            this.memberServices = memberServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var tempResult = TempData["Result"];
            var Member = await memberServices.GetAllMembersAsync(ct);
            return View(Member);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid) { return View(nameof(Create), model); }

            var Result =await memberServices.CreateMemberAsync(model, ct) ;

            if (Result)
            {
                TempData["Success"] = "Member Created Sucessfully";
            }
            else
            {
                TempData["Failed"] = "Failed To create Member";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var Member = await memberServices.GetMemberDetailsAsync(id, ct);

            if(Member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }    
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await memberServices.GetMemberHealthRecordAsync(id, ct);

            if(healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record not found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
    }
}

