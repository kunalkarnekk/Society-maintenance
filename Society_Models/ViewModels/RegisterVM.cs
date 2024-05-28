using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }
        [Required]
        [DisplayName("UserName")]
        public string UserName { get; set; }

        public int SocietyId { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm Pasword")]
        [DataType(DataType.Password)]
        [Compare("Password" , ErrorMessage ="Password and Compared Password do not match")]
        public string ConfirmPassword { get; set; }


        public IEnumerable<SelectListItem> RoleList { get; set; }

        [Required]
        public string Image { get; set; }
        public string societyName { get; set; }

        public string Role { get; set; }
        [Required]
        public string Address { get; set; }

        public int PinCode { get; set; }

        [Required]
        public string State { get; set; }
        public string City { get; set; }
        public string LandMark { get; set; }
        public int BuiltYear { get; set; }

        public int RegistrationNo { get; set; }

    }
}
