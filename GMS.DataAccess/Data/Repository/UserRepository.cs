using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using Gym_Management_System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.DataAccess.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

        public void LockUser(string userId)
        {
            var userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        public void UnlockUser(string userId)
        {
            var userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            userFromDb.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
