using GymSystem.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        //2 Actions :Index(All Plans) Details(Plan Details)
        
        private GymDbContext dbContext = new GymDbContext();
        public async Task<IActionResult> Index()
        {
            var plans =await dbContext.Plans.ToListAsync();
            return View(plans); // view with model
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await dbContext.Plans.FirstOrDefaultAsync(p=>p.Id == id);
            if (plan == null)
                RedirectToAction(nameof(Index));
            
            return View(plan);
        }
    }
}
