using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models
{
    public class FlatOwner
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public int FlatNumber { get; set; }
        public OwnerType OwnerType { get; set; }
        public bool IsSocietyMember { get; set; }
        public bool IsActive { get; set; } = true;

        public int SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public Society? Society { get; set; }

        public List<Maintenance> Maintenance { get; set; }

        public string? DesiginationName { get; set; }

    }


    public enum OwnerType
    {
        Rent,
        Own
    }

}
