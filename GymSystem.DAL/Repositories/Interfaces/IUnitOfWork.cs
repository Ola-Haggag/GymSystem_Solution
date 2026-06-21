using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public Task<int> CompleteAsync();
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
    
     public ISessionRepository SessionRepository { get; }

    }
}
