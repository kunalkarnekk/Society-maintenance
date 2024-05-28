using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models
{
    public class Society
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Image { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }

        public int PinCode { get; set; }

        [Required]
        public string? State { get; set; }
        public string? City { get; set; }
        public string? LandMark { get; set; }

        public int BuiltYear { get; set; }
        public int? RegistrationNo { get; set; }

        public string? CreatationDate { get; set; }

        public Settings? settings { get; set; }

        public List<Expense>? Expenses { get; set; }
    }
}
