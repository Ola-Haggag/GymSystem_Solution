using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionServices(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End date must be after start date");
            if (model.StartDate <= DateTime.Now) return Result.Validation("start date must be in the future");

            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetById(model.TrainerId , ct);
            if(Trainer is null) return Result.NotFound("Trainer not found");
            
            var CategoryRepo = unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetById(model.CategoryId, ct);
            if (Category is null) return Result.NotFound("Category not found");

            var session = mapper.Map<CreateSessionViewModel, Session>(model);
            var sessionRepo = unitOfWork.GetRepository<Session>();
            
            sessionRepo.Add(session);
            
            var RowEffected = await unitOfWork.CompleteAsync();
           
            return RowEffected > 0 ? Result.Ok() : Result.Fail("Failed to create session ");
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct)
        {
            var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);

            if (!sessions.Any())
            {
                return null;
            }
            sessions = sessions.OrderByDescending(x => x.StartDate);

            var MappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
             foreach(var session in MappedSessions)
             {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id , ct);
             }
            return MappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var Category = await unitOfWork.GetRepository<Category>().GetAll(false, ct);

            return mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Category);

        }

        public async Task<SessionViewModel?> GetSessionById(int SessionId, CancellationToken ct)
        {
            var session = await unitOfWork.GetRepository<Session>().GetById(SessionId);
            return mapper.Map<Session, SessionViewModel > (session);
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int SessionId, CancellationToken ct)
        {
            var Session = await unitOfWork.SessionRepository.GetSessionsByIdWithTrainerAndCategoryAsync(SessionId, ct);

            if(Session == null)
            {
                return null;                    
                
            }
            var MappedSession = mapper.Map<Session, SessionViewModel>(Session);
            MappedSession.AvailableSlots = MappedSession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(MappedSession.Id, ct);

            return MappedSession;
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int sessionId, CancellationToken ct)
        {
            var Session = await unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
            if (Session is null)
                return null;

            if(await IsSessionValidForUpdateAsync(Session, ct)) return null;

            return mapper.Map<UpdateSessionViewModel>(Session);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var Trainer = await unitOfWork.GetRepository<Trainer>().GetAll(false, ct);
            
            return mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(Trainer);

        }

        public async Task<Result> RemoveSessionAsync(int SessionId, CancellationToken ct)
        {
            var Repo = unitOfWork.GetRepository<Session>();

            var Session = await Repo.GetById(SessionId, ct);

            if (Session is null) return Result.NotFound("Session not found");

            if(Session.EndDate >= DateTime.Now)
                return Result.Fail("Can not delete a session that has not yet ended");

                var bookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(SessionId, ct);

                if(bookedCount > 0)
                {
                    return Result.Fail("Can not delete a session that has Bookings");
                }
                Repo.Delete(SessionId);
           
            var AffectedRows = await unitOfWork.CompleteAsync();

            return AffectedRows > 0 ? Result.Ok() : Result.Fail("Failed to Remove Session");
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var SessionRepo = unitOfWork.GetRepository<Session>();
            var Session = await SessionRepo.GetById(id, ct);

            if (Session is null)
                return Result.NotFound("Session Not Found");
            if (Session.StartDate <= DateTime.Now)
                return Result.Fail("Can not Edit a session that has already started");

            var BookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(Session.Id, ct);
            if (BookedCount > 0)
                return Result.Fail("Can not Edit a session that has booked slots");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date");
            if (model.StartDate <= DateTime.Now) 
                return Result.Validation("start date must be in the future");



            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetById(model.TrainerId, ct);
            if (Trainer is null) return Result.NotFound("Trainer not found");

            Session.UpdatedAt = DateTime.Now;

            mapper.Map(model, Session);
            SessionRepo.Update(Session);

            var EffectedRows = await unitOfWork.CompleteAsync();

            return EffectedRows > 0 ? Result.Ok() : Result.Fail("Failed to create session ");

        }

        private async Task<bool> IsSessionValidForUpdateAsync(Session session, CancellationToken ct)
        {
            if (session.StartDate <= DateTime.Now)
                return false;

            var Booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            return Booked == 0;
        }  
    }
}
