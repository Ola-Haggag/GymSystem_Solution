using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Entities
{
    public class Member:GymUser
    {
        public string Photo { get; set; } = null!;

        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<MemberShip> MemberShips = new HashSet<MemberShip>();
       
        public ICollection<Booking> Bookings = new HashSet<Booking>();
    }
}
