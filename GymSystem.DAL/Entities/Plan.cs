using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Entities
{
    public class Plan : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
        
        [Required, MaxLength(200)]
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }

        public ICollection<MemberShip> MemberShips = new HashSet<MemberShip>();

    }
}
