using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models.ViewModels
{
    public class MaintenanceVM
    {
        public Maintenance? Maintenance { get; set; }
        public List<Maintenance>? Maintenance1 { get; set; }
        public List<SelectListItem>? MonthsList { get; set; }
        public List<int>? YearsList { get; set; }
        public IEnumerable<SelectListItem>? FlatOwnersList { get; set; }
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
