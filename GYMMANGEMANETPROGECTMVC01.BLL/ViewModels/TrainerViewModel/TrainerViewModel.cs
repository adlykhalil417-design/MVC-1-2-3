using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.BLL.ViewModels.TrainerViewModel
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; } 
        public string Specialties { get; set; }
        public string DateOfBirth { get; set; } 
        public string Address { get; set; } 
    }
}
