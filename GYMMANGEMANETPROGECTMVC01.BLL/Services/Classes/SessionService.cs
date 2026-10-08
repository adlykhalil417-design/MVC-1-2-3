using AutoMapper;
using GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.SessionViewModels;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using GYMMANGEMANETPROGECTMVC01.DAL.Models.Enums;
using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Classes;
using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.BLL.Services.Classes
{
    public  class SessionService : ISessionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork uow ,IMapper mapper){
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel vm, CancellationToken ct = default)
        {
          if(vm.EndDate<=vm.StartDate)return false;
          if(vm.StartDate<=DateTime.Now)return false;
          if(vm.Capacity<1 || vm.Capacity>25)return false;

          var trainer=await _uow.GetRepository<Trainer>().GetByIdAsync(vm.TrainerId);
            if(trainer==null)return false;

            var category = await _uow.GetRepository<Category>().GetByIdAsync(vm.CategoryId);
            if (category==null)return false;

            var isvalid = Enum.TryParse<Speciality>(category.CategoryName, true, out var CatagorySpeciality); 

            if(!isvalid || trainer.Speciality!=CatagorySpeciality) return false;
            var session = _mapper.Map<Session>(vm);
            _uow.GetRepository<Session>().Add(session);
            var res=await _uow.SaveChangesAsync(ct);
            return res > 0;

        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions= await _uow.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (sessions == null || !sessions.Any()) return null;
            var mapsession = sessions.Select(s=>new SessionViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Capacity = s.Capacity,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                TrainerName=s.Trainer.Name,
                CategoryName=s.Category.CategoryName


            });
            foreach( var session in mapsession)
            {
               session.AvailableSlots = session.Capacity - (await _uow.SessionRepository.GetcountofBookedSlotsAsync(session.Id));
            }
            return mapsession;

        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropdownAsync(CancellationToken ct = default)
        {
            var res=await _uow.GetRepository<Category>().GetAllAsync(ct:ct);

           return _mapper.Map<IEnumerable<CategorySelectViewModel>>(res);
            

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrinersForDropdownAsync(CancellationToken ct = default)
        {
            var res = await _uow.GetRepository<Trainer>().GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(res);
        }
    }
}
