using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models.ViewModels
{
    public class RegisterUserVM
    {
        [Required]
        public string? Name { get; set; }
       
        public String? Role { get; set; }

        public IEnumerable<SelectListItem>? FlatOwners { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
