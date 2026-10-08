using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.MemberViewModels;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using AutoMapper;

namespace GYMMANGEMANETPROGECTMVC01.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
       
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork uow ,IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel memberViewModel, CancellationToken ct = default)
        {
            var memberRepository = _uow.GetRepository<Member>();
            //Check Email
            //var membe
            //Check Phone
            var emailExists = await memberRepository.AnyAsync(m => m.Email == memberViewModel.Email);
            var phoneExists = await memberRepository.AnyAsync(m => m.Phone == memberViewModel.Phone);

            if (emailExists || phoneExists) return false;

         var member=_mapper.Map<Member>(memberViewModel);
              memberRepository.Add(member);
            var res =await _uow.SaveChangesAsync(ct);
            return res>0;

            }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _uow.GetRepository<Member>().GetAllAsync(ct:ct);

            if (!members.Any()) return [];


         var VModel=_mapper.Map<IEnumerable<MemberViewModel>>(members);
            return VModel;  
        }
        public async Task<MemberViewModel> GetMemberDetailsByIdAsync(int id,CancellationToken ct = default)
        {
            var member=await _uow.GetRepository<Member>().GetByIdAsync(id);
            if (member is null) return null;
            var VModel=_mapper.Map<MemberViewModel>(member);
            var activemembership = await _uow.GetRepository<MemberShip>().FristOrDefaultAsync(m=>m.MemberId == id&& m.EndDate>DateTime.Now);
            if(activemembership is not null)
            {
                VModel.PlanName =activemembership.Plan.Name;
                VModel.MembershipStartDate = activemembership.CreatedAt.ToString("yyyy-MM-dd");
                VModel.MembershipEndDate = activemembership.EndDate.ToString("yyyy-MM-dd");


            }
            return VModel;



        }
        public async Task<bool> RemoveMemberAsync(int memberid,CancellationToken ct = default)
        {
            var member =await _uow.GetRepository<Member>().GetByIdAsync(memberid);
            if (member == null) return false;

            //future booking
            var hasfuturebooking = await _uow.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberid && b.CreatedAt > DateTime.Now, ct);
            if (hasfuturebooking) return false;

             _uow.GetRepository<Member>().Delete(member);
            var res=await _uow.SaveChangesAsync( ct);
            return res>0;
        }
        public async Task<HealthRecordViewModel>GetHealthRecordAync(int memberid,CancellationToken ct = default)
        {
            var record=await _uow.GetRepository<HealthRecord>().FristOrDefaultAsync(h=>h.MemberId == memberid);
            if (record is null) return null;
            var VModel = _mapper.Map<HealthRecordViewModel>(record);
            return VModel;
        }
        public async Task<MemberToUpdateViewModel>GetMemberToUpdateASync(int id,CancellationToken ct = default)
        {
            var member =await _uow.GetRepository<Member>().GetByIdAsync(id);
            if (member is null) return null;
            var VModel=_mapper.Map<MemberToUpdateViewModel>(member);
            return VModel;

        }
        public async Task<bool>UpdateMemberDetailsAsync(int id,MemberToUpdateViewModel memberVM,CancellationToken ct = default) { 
        
        var member=await _uow.GetRepository<Member>().GetByIdAsync(id,ct);
            if (member is null) return false;

            var memberRepository = _uow.GetRepository<Member>();
            //Check Email
            //var membe
            //Check Phone
            var emailExists = await memberRepository.AnyAsync(m => m.Email == memberVM.Email && m.Id != id);
            var phoneExists = await memberRepository.AnyAsync(m => m.Phone == memberVM.Phone && m.Id!=id);
            if (emailExists || phoneExists) return false;

            _mapper.Map(memberVM,member);
            member.UpdatedAt = DateTime.Now;
            _uow.GetRepository<Member>().Update(member);
            var res=await _uow.SaveChangesAsync(ct);
            return res > 0;
        }
    }
}
