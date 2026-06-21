using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
        public Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default);
        Task<SessionViewModel?> GetSessionByIdAsync(int SessionId, CancellationToken ct);
        Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int sessioId , CancellationToken ct);
        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);

        Task<SessionViewModel?> GetSessionById(int SessionId, CancellationToken ct);

        Task<Result> RemoveSessionAsync(int SessionId, CancellationToken ct);
    
    }
}
