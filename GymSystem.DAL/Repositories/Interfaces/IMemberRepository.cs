using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IMemberRepository:IGenericRepository<Member>
    {
        //Task<IEnumerable<Member>> GetAll(bool isTracked, CancellationToken ct = default);

        ////Task<Member?> GetById(int id);
        //Task<Member?> GetById(int id, CancellationToken ct = default);
        //void Add(Member member);
        //void Update(Member member);
        //void Delete(int id);
        //Task<int> CompleteAsync();
    }
}
