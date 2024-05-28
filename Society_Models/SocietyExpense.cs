using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models
{
    public class SocietyExpense
    {
        [Key]
        public int ExpenseId { get; set; }
        public string? ExpenseName { get; set; } = string.Empty;
        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public List<Expense>? Expenses { get; set; }
    }
}
