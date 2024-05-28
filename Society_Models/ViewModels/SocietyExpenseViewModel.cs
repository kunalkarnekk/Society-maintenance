using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models.ViewModels
{
    public class SocietyExpenseViewModel
    {
        public SocietyExpense? SocietyExpense { get; set; }
        public List<SelectListItem>? ExpensesList { get; set; }
    }
}
