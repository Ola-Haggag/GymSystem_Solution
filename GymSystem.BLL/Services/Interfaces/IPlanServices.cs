using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IPlanServices
    {
        public Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct);
        Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct);
        Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default);

    }
}
