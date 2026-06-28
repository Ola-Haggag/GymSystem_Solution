using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.AnalyticsViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class AnalyticsServices : IAnalyticsServices
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var Sessions = await unitOfWork.GetRepository<Session>().GetAll(false, ct);

            var totalMembers = await unitOfWork.GetRepository<Member>().CountAsync(ct:ct);
            var totalTrainers = await unitOfWork.GetRepository<Trainer>().CountAsync(ct:ct) ;
            var ActiveMembers = await unitOfWork.GetRepository<MemberShip>().CountAsync( m => m.EndDate > DateTime.Now,ct);

            return new AnalyticsViewModel
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = ActiveMembers,
                UpcomingSessions = Sessions.Count(x => x.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now),
                CompletedSessions = Sessions.Count(x=> x.EndDate < DateTime.Now)
                
            };
        }
    }
}
