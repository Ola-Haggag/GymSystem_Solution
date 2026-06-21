using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository:IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct);
        Task<Session> GetSessionsByIdWithTrainerAndCategoryAsync(int SessionId , CancellationToken ct);

        Task<int> GetCountOfBookedSlotAsync(int SessionId, CancellationToken ct);
    }
}
