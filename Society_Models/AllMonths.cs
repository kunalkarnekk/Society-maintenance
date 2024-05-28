using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models
{
    public class AllMonths
    {
        public int MonthNumber { get; set; }
        public string? MonthName { get; set; }

        public static List<AllMonths> GetAllMonths()
        {
            List<AllMonths> months = new List<AllMonths>();

            for (int i = 1; i <=12; i++)
            {
                months.Add(new AllMonths
                {
                    MonthNumber = i,
                    MonthName = new DateTime(2000 , i , 1).ToString("MMMM"),
                });
            }

            return months;

        }
    }
}
