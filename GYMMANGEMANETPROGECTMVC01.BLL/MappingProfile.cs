using AutoMapper;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.MemberViewModels;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.SessionViewModels;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.BLL
{
    
    public class MappingProfile:Profile
    {
        
        public MappingProfile()
        {
            MapMember();

            Mapsession();

        }
        

        private void MapMember()
        {
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));
            

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                  .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));
            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    BuildingNumber = src.BuildingNumber
                }));

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
        }
        
        private void Mapsession()
        {
            // src:session
            //dest:sessionviewmodel
            CreateMap< /* src*/ Session,/* dest*/  SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                 .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name));
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>().ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.CategoryName));
            
            CreateMap<Session,UpdateSessionViewModel>().ReverseMap();
                }
        


    }
}
