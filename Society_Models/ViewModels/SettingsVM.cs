using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models.ViewModels
{
    public class SettingsVM
    {
        public Settings? Settings { get; set; }

        public IEnumerable<SelectListItem>? SocietyList { get; set; }
    }
}
