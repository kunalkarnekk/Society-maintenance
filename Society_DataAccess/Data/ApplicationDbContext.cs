using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Society_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Society> Society { get; set; }
        public DbSet<FlatOwner> FlatOwner { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }

        public DbSet<Disignation> Disignations { get; set; }

        public DbSet<Settings> Settings { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<SocietyExpense> SocietyExpenses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

    }
}
