using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = " Owner Maintenance Amount")]
        public float OwnerMaintenanceCost { get; set; }
        [Required]
        [Display(Name = " Rent Maintenance Amount")]
        public float RentMaintenanceCost { get; set; }
        [Required]
        [Display(Name = "Fine Amount")]
        public float FineAmount { get; set; }
        [Required]
        [Display(Name = "Maintenance Date")]
        public DateOnly MaintenanceDate { get; set; }


        public int SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public Society? Society { get; set; }
    }
}
