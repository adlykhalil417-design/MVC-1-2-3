using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.DAL.Models
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; } = default!;

        public ICollection<Session> Sessions { get; set; } = default!;
    }
}
