using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionServices sessionServices;

        public SessionController(ISessionServices sessionServices)
        {
            this.sessionServices = sessionServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await sessionServices.GetAllSessionsAsync(ct);
            return View(sessions);
        }
        public async Task <IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownAsync(ct);
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(CreateSessionViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }
            var result = await sessionServices.CreateSessionAsync(model, ct);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropDownAsync(ct);
            return View(model);
        }
        private async Task PopulateDropDownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList( await sessionServices.GetTrainersForDropDownAsync(ct) ,"Id", "Name" );
            ViewBag.Categories  = new SelectList(await sessionServices.GetCategoriesForDropDownAsync(ct), "Id" , "CategoryName");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var Session = await sessionServices.GetSessionByIdAsync(id, ct);
            if(Session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var Session = await sessionServices.GetSessionToUpdateAsync(id, ct);
            if(Session is null) 
            {
                TempData["ErrorMessage"] = "Session can not be edit, it is not found";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownAsync(ct);
            return View(Session);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }
            var result = await sessionServices.UpdateSessionAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;

            await PopulateDropDownAsync(ct);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Session = await sessionServices.GetSessionById(id, ct);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await sessionServices.RemoveSessionAsync(id, ct);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Success ? "Session Deleted Successfully!" : result.Error;

            return RedirectToAction(nameof(Index));
        }
    }
}
