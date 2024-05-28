using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required]
        public string? ExpenseName { get; set; } = string.Empty;

        [Required]
        public decimal ExpenseAmount { get; set; }

        public string? Description { get; set; } = string.Empty;
        public Frequency Frequency { get; set; } 

        [Required]
        public string? ExpenseBy { get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.Now;

        public int SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public Society? Society { get; set; }


    }

    public enum Frequency
    {
        Monthly,
        Quarterly,
        HalfYearly,
        Annual
    }


    
   
}
