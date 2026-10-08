
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);

        Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default);

        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default);

        Task<bool> CreateTrainerAsync(CreateTrainerViewModel createTrainer, CancellationToken ct = default);

        Task<bool> UpdateTrainerDetailsAsync(TrainerToUpdateViewModel updatedTrainer, int trainerId, CancellationToken ct = default);

        Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default);
    }
}
