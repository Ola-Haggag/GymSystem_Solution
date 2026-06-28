using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {
        private readonly ITrainerServices trainerServices;

        public TrainerController(ITrainerServices trainerServices)
        {
            this.trainerServices = trainerServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await trainerServices.GetAllTrainersAsync(ct);
            return View(trainers);
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await trainerServices.CreateTrainerAsync(model, ct);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await trainerServices.GetTrainerById(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainer = await trainerServices.GetTrainerToUpdateAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "trainer can not be edit, it is not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await trainerServices.UpdateTrainerAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Trainer = await trainerServices.GetTrainerById(id, ct);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await trainerServices.RemoveTrainerAsync(id, ct);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Success ? "Trainer Deleted Successfully!" : result.Error;

            return RedirectToAction(nameof(Index));
        }
    }
}
