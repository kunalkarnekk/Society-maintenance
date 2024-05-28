using Society_DataAccess.Data;
using Society_DataAccess.Repository.IRepository;
using Society_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_DataAccess.Repository
{
    public class SocietyRepository : Repository<Society> , ISocietyRepository
    {
        private readonly ApplicationDbContext _context;
        public SocietyRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
