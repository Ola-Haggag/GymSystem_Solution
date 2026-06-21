using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanServices planServices;

        //2 Actions :Index(All Plans) Details(Plan Details)
        //private GymDbContext dbContext = new GymDbContext();

        //private readonly IGenericRepository<Plan> planRepository;
        public PlanController(IPlanServices planServices)
        {
            this.planServices = planServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planServices.GetAllPlansAsync(ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var Plan = await planServices.GetPlanByIdAsync(id, ct);
            if (Plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await planServices.GetPlanToUpdateAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "plan can not be edit, it is not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await planServices.UpdatePlanAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "plan Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;

            return View(model);
        }
    }
}
