using GymSystem.DAL.Contexts;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        //2 Actions :Index(All Plans) Details(Plan Details)
        //private GymDbContext dbContext = new GymDbContext();

        private readonly IPlanRepository planRepository;
        public PlanController(IPlanRepository _planRepository)
        {
            planRepository = _planRepository;
        }
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await planRepository.GetAll(false, token);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planRepository.GetById(id, token);
            if (plan == null)
                RedirectToAction(nameof(Index));
            
            return View(plan);
        }
    }
}
