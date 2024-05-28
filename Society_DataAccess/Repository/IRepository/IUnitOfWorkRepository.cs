using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_DataAccess.Repository.IRepository
{
    public interface IUnitOfWorkRepository
    {
        public IDisignationRepository Disignation { get;}
        public ISettingsRepository   Settings { get; }
        public ISocietyRepository Society { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public ISocietyExpenseRepository SocietyExpense { get; }





        public void Save();
    }
}
