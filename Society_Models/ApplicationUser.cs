using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public int SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public Society Society { get; set; }
    }
}
