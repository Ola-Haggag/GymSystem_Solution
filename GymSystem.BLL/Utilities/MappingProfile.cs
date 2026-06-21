using AutoMapper;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystem.BLL.ViewModels.TrainerViewModels;

namespace GymSystem.BLL.Utilities
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            MapSession();
            MapPlan();
            MapTrainer();
        }
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name));

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();
        }
        private void MapTrainer()
        {
            CreateMap<Trainer, TrainerViewModel>();

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }));

            CreateMap<Trainer, TrainerToUpdateViewModel>().ReverseMap();
        }
    }
}
