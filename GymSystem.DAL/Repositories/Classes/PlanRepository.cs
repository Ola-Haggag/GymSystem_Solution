using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbContext;
        public PlanRepository(GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<IEnumerable<Plan>> GetAll(bool isTracked, CancellationToken ct=default)
        {
            var plans = isTracked ? dbContext.Plans : dbContext.Plans.AsNoTracking();
            return await plans.ToListAsync();
        }

        public async Task<Plan?> GetById(int id, CancellationToken ct = default)
        {
            var plan = await dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
            return plan;
        }
        public void Add(Plan plan)
        {
            dbContext.Plans.Add(plan);
        }
        public void Update(Plan plan)
        {
            dbContext.Plans.Update(plan);
        }
        public void Delete(int id)
        {
            var product = dbContext.Plans.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                dbContext.Plans.Remove(product);
            }
        }
        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
