using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ITrainerServices
    {
        public Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct);
        Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);

        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int TrainerId, CancellationToken ct);
        Task<Result> UpdateTrainerAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default);

        Task<TrainerViewModel?> GetTrainerById(int TrainerId, CancellationToken ct);

        Task<Result> RemoveTrainerAsync(int TrainerId, CancellationToken ct);
    }
}
