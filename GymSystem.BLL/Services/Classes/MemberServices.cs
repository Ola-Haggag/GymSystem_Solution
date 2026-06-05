using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IGenericRepository<Member> memberRepository;
        private readonly IGenericRepository<MemberShip> memberShipRepository;
        private readonly IGenericRepository<Plan> planRepository;
        private readonly IGenericRepository<HealthRecord> healthRecordRepository;
        private readonly IGenericRepository<Booking> bookingRepository;

        public MemberServices(IGenericRepository<Member> memberRepository, IGenericRepository<MemberShip>memberShipRepository , 
            IGenericRepository<Plan> PlanRepository
            ,IGenericRepository<HealthRecord> HealthRecordRepository,
            IGenericRepository<Booking> bookingRepository)
        {
            this.memberRepository = memberRepository;
            this.memberShipRepository = memberShipRepository;
            this.planRepository = PlanRepository;
            healthRecordRepository = HealthRecordRepository;
            this.bookingRepository = bookingRepository;
        }
   
        //get
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await memberRepository.GetAll(false, ct);
            if (!members.Any()) return [];

            var MemberViewModel = members.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString(),
            });
            return MemberViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            //Member + MemberShip + plan
            var member = await memberRepository.GetById(memberId, ct);
            if (member == null) return null;
            var MemberVM = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
            //MemberShip
            var ActiveMemberShip = await memberShipRepository.FirstOrDefaultAsync(mb => mb.MemberId == memberId && mb.EndDate > 
            DateTime.Now, false,ct);

            if(ActiveMemberShip is not null)
            {
                var ActivePlan = await planRepository.GetById(ActiveMemberShip.PlanId, ct);
                MemberVM.PlanName = ActivePlan?.Name;
                MemberVM.MembershipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                MemberVM.MembershipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }
            return MemberVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var Record = await healthRecordRepository.FirstOrDefaultAsync(r => r.MemberId == memberId, false,ct);
            if (Record is null)
            {
                return null;
            }
            return new HealthRecordViewModel()
            {
                Weight = Record.Weight,
                Height = Record.Height,
                BloodType = Record.BloodType,
                Note = Record.Note,
                 

            };
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(memberId, ct);
            if(member is null)
            {
                return null;
            }
            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Street = member.Address.Street,
                City = member.Address.City,
                BuildingNumber = member.Address.BuildingNumber,
                Photo = member.Photo
            };
        }
        //post
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            //get all
            //any (expression<func<Entity, bool>> predicate)
            var EmailExists = await memberRepository.AnyAsync(m => m.Email == model.Email, ct);
            var PhoneExists = await memberRepository.AnyAsync(m => m.Phone == model.Phone, ct);
       
            if(EmailExists || PhoneExists)
            {
                return false;
            }
            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street,
                },
                HealthRecord = new HealthRecord()
                {
                   Height = model.HealthRecordViewModel.Height,
                   Weight = model.HealthRecordViewModel.Weight,
                   BloodType = model.HealthRecordViewModel.BloodType,
                   Note = model.HealthRecordViewModel.Note,
                }
            };
            memberRepository.Add(member);
            var Result = await memberRepository.CompleteAsync();
            return Result > 0;
        }
        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(id , ct);
            if (member is null) { return false; }

            // email ==member.email && id != member.id

            if (await memberRepository.AnyAsync(m => m.Email == model.Email
            && m.Id != id)) return false;
            if (await memberRepository.AnyAsync(m => m.Phone == model.Phone
            && m.Id != id)) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;
            memberRepository.Update (member);

            var result = await memberRepository.CompleteAsync();
            return result > 0;
        }
        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(memberId , ct);
            if (member is null) { return false;}

            var HasfutureSessions = await bookingRepository.AnyAsync(
                b=>b.MemberId == memberId && b.Session.EndDate > DateTime.Now);

            if (HasfutureSessions)
            {
                return false;
            }
            memberRepository.Delete(memberId);

            var result = await memberRepository.CompleteAsync();
            return result > 0; 
        }
    }
}
