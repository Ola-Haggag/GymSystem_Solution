using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class TrainerServices : ITrainerServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerServices(IUnitOfWork unitOfWork , IMapper mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var Trainer = mapper.Map<CreateTrainerViewModel, Trainer>(model);
            var TrainerRepo = unitOfWork.GetRepository<Trainer>();

            TrainerRepo.Add(Trainer);

            var RowEffected = await unitOfWork.CompleteAsync();

            return RowEffected > 0 ? Result.Ok() : Result.Fail("Failed to create Trainer ");
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct)
        {
            var Trainers = await unitOfWork.GetRepository<Trainer>().GetAll(false, ct);
            return mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);
        }

        public async Task<TrainerViewModel?> GetTrainerById(int TrainerId, CancellationToken ct)
        {
            var Trainer = await unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            return mapper.Map<Trainer, TrainerViewModel>(Trainer);
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int TrainerId, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer is null)
                return null;

            return mapper.Map<TrainerToUpdateViewModel>(trainer);
        }

        public async Task<Result> RemoveTrainerAsync(int TrainerId, CancellationToken ct)
        {
            var Repo = unitOfWork.GetRepository<Trainer>();

            var trainer = await Repo.GetById(TrainerId, ct);

            if (trainer is null) return Result.NotFound("trainer not found");

            Repo.Delete(TrainerId);

            var AffectedRows = await unitOfWork.CompleteAsync();

            return AffectedRows > 0 ? Result.Ok() : Result.Fail("Failed to Remove trainer");
        }

        public async Task<Result> UpdateTrainerAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var trainer = await TrainerRepo.GetById(id, ct);

            if (trainer is null)
                return Result.NotFound("Session Not Found");

            trainer.UpdatedAt = DateTime.Now;

            mapper.Map(model, trainer);
            TrainerRepo.Update(trainer);

            var EffectedRows = await unitOfWork.CompleteAsync();

            return EffectedRows > 0 ? Result.Ok() : Result.Fail("Failed to update trainer ");
        }
    }
}
