using Microsoft.AspNetCore.Identity;


namespace GYMMANGEMANETPROGECTMVC01.DAL.Models
{

    
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }=default!;
        public string LastName { get; set; } = default!;
    }
}
