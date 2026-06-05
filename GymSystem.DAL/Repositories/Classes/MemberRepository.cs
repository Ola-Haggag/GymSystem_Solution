using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class MemberRepository : GenericRepository<Member> ,IMemberRepository
    {
        private readonly GymDbContext dbContext;

        public MemberRepository(GymDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
        //void IMemberRepository.Add(Member member)
        //{
        //    dbContext.Members.Add(member);
        //}

        //public async Task<int> CompleteAsync()
        //{
        //    return await dbContext.SaveChangesAsync();
        //}

        //void IMemberRepository.Delete(int id)
        //{
        //    var member = dbContext.Members.FirstOrDefault(p => p.Id == id);
        //    if (member != null)
        //    {
        //        dbContext.Members.Remove(member);
        //    }
        //}

        // public async Task<IEnumerable<Member>> GetAll(bool isTracked, CancellationToken ct)
        // {
        //    var Members = isTracked ? dbContext.Members : dbContext.Members.AsNoTracking();
        //    return await Members.ToListAsync();
        // }

        //public async Task<Member?> GetById(int id, CancellationToken ct)
        //{
        //    var Member = await dbContext.Members.FirstOrDefaultAsync(p => p.Id == id);
        //    return Member;
        //}

        //void IMemberRepository.Update(Member member)
        //{
        //    dbContext.Members.Update(member);
        //}
    }
}
