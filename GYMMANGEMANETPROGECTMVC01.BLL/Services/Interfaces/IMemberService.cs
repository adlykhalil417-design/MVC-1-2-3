using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel memberViewModel, CancellationToken ct = default);
        Task<MemberViewModel> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default);
        Task<bool> RemoveMemberAsync(int memberid, CancellationToken ct = default);
         Task<HealthRecordViewModel> GetHealthRecordAync(int memberid, CancellationToken ct = default);
         Task<MemberToUpdateViewModel> GetMemberToUpdateASync(int id, CancellationToken ct = default);
        Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel memberVM, CancellationToken ct = default);

    }
}
