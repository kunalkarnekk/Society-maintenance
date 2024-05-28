using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models.ViewModels
{
    public class LoginUsersVM
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name ="Remeber Me")]
        public bool RememberMe { get; set; }

        //public IEnumerable<SelectListItem>? FlatOwners { get; set; }
        //public IEnumerable<SelectListItem>? Rols { get; set; }

    }
}
