using GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.TrainerViewModel;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
     
        private readonly IUnitOfWork _uow;

        public TrainerService(IUnitOfWork uow)
        {
          
            _uow = uow;
        }
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _uow.GetRepository<Trainer>().GetAllAsync(ct: ct);

            return trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Speciality.ToString()
            });
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel vm, CancellationToken ct = default)
        {

            try
            {
                if (await IsEmailExists(vm.Email) || await IsPhoneExists(vm.Phone)) return false;
                var TrainerEntity = new Trainer()
                {
                    Name = vm.Name,
                    DateOfBirth = vm.DateOfBirth,
                    Gender = vm.Gender,
                    Phone = vm.Phone,
                    Email = vm.Email,
                    Speciality = vm.Specialties,
                    Address = new Address()
                    {
                        City = vm.City,
                        Street = vm.Street,
                        BuildingNumber = vm.BuildingNumber
                    }
                };

                 _uow.GetRepository<Trainer>().Add(TrainerEntity) ;
                var res = await _uow.SaveChangesAsync();
                return res > 0;

            }
            catch (Exception)
            {
                return false;
            }



        }

      
        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _uow.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return null;

            return new TrainerViewModel()
            {
                Name = trainer.Name,
                Specialties = trainer.Speciality.ToString(),
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
            };
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _uow.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Specialties = trainer.Speciality,
            };
        }

        public async  Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainerToRemove = await _uow.GetRepository<Trainer>().GetByIdAsync(trainerId);
            if (trainerToRemove is null || await HasActiveSessions(trainerId, ct)) return false;

            _uow.GetRepository<Trainer>().Delete(trainerToRemove);
            var res=await _uow.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<bool> UpdateTrainerDetailsAsync(TrainerToUpdateViewModel model, int trainerId, CancellationToken ct = default)
        {
            var trainer = await _uow.GetRepository<Trainer>().GetByIdAsync(trainerId);
            if (trainer is null) return false;

            if (await _uow.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct))
                return false;

            if (await _uow.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct))
                return false;

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Speciality = model.Specialties;
            trainer.UpdatedAt = DateTime.Now;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.Address.BuildingNumber = model.BuildingNumber;

              _uow.GetRepository<Trainer>().Update(trainer) ;
            var res = await _uow.SaveChangesAsync(ct);
            return res>0;
        }
        #region Helper Methods
        private async Task<bool> IsEmailExists(string email)
        {
            var existing = await _uow.GetRepository<Trainer>().AnyAsync(m => m.Email == email);
            return existing;
        }

        private async Task<bool> IsPhoneExists(string phone)
        {
            var existing = await _uow.GetRepository<Trainer>().AnyAsync(m => m.Phone == phone);
            return existing;
        }

        private async Task<bool> HasActiveSessions(int Id, CancellationToken ct)
        {
            var activeSessions = await _uow.GetRepository<Session>().AnyAsync(s => s.TrainerId == Id && s.StartDate > DateTime.Now, ct);
            return activeSessions;
        }
        #endregion
    }
}
