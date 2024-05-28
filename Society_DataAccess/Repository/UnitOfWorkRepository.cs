using Society_DataAccess.Data;
using Society_DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_DataAccess.Repository
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _context;

       
        public IDisignationRepository Disignation { get; private set; }
        public ISettingsRepository Settings { get; private set; }
        public ISocietyRepository Society { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ISocietyExpenseRepository SocietyExpense { get; private set; }





        public UnitOfWorkRepository(ApplicationDbContext context)
        {
            _context = context;

            Disignation = new DisignationRepository(_context);
            Settings = new SettingsRepository(_context);
            Society = new SocietyRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            SocietyExpense = new SocietyExpenseRepository(_context);

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
