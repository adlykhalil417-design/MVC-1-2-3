using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.Services.Interfaces;
using GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.MemberViewModels;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;

namespace GYMMANGEMANETPROGECTMVC01.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository=memberRepository;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel memberViewModel, CancellationToken ct = default)
        {
            //Check Email
            //var membe
            //Check Phone
            var emailExists = await _memberRepository.AnyAsync(m => m.Email == memberViewModel.Email);
            var phoneExists = await _memberRepository.AnyAsync(m => m.Phone == memberViewModel.Phone);

            if (emailExists || phoneExists) return false;

            var member = new Member()
            {
                Name=memberViewModel.Name,
                Email=memberViewModel.Email,
                Phone=memberViewModel.Phone,
                DateOfBirth=memberViewModel.DateOfBirth,
                Gender=memberViewModel.Gender,
              Address=new Address()
              {
                  City=memberViewModel.City,
                  Street=memberViewModel.Street,
                  BuildingNumber=memberViewModel.BuildingNumber
              },
              HealthRecord=new HealthRecord()
              {
                  BloodType=memberViewModel.HealthRecordViewModel.BloodType,
                  Note=memberViewModel.HealthRecordViewModel.Note,
                   Height=memberViewModel.HealthRecordViewModel.Height,
                   Weight=memberViewModel.HealthRecordViewModel.Weight    
              }

            };
             var res= await _memberRepository.AddAsync(member);

            return res>0;

            }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync(ct:ct);

            if (!members.Any()) return [];


            return members.Select(member => new MemberViewModel()
            {
                Id=member.Id,
                Name=member.Name,
                Email=member.Email,
                Gender=member.Gender.ToString(),
                Phone=member.Phone,
                Photo=member.Photo
            });
        }
    }
}
