using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var Plans = await _unitOfWork.GetRepository<Plan>().GetAll(false, ct);
            return _mapper.Map<IEnumerable<PlanViewModel>>(Plans);
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetById(PlanId, ct);
            return plan is null ? null : _mapper.Map<PlanViewModel>(plan);
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetById(PlanId, ct);
            if(plan is null || !plan.IsActive)
                return null;
            return _mapper.Map<UpdatePlanViewModel>(plan);

        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var plan = await PlanRepo.GetById(id, ct);

            if (plan is null)
                return Result.NotFound("plan not found");


            _mapper.Map(model, plan);
            PlanRepo.Update(plan);

            var EffectedRows = await _unitOfWork.CompleteAsync();

            return EffectedRows > 0 ? Result.Ok() : Result.Fail("Failed to update  plan ");
        }
    }
}
