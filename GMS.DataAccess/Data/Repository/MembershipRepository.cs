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
    public class MembershipRepository : Repository<Membership>, IMembershipRepository
    {
        private readonly ApplicationDbContext _db;
        public MembershipRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

        public void Update(Membership membership)
        {
            var objFromDb = _db.Memberships.FirstOrDefault(c => c.Id == membership.Id);
            objFromDb.Name = membership.Name;
            objFromDb.Price = membership.Price;
            objFromDb.Description = membership.Description;
            _db.SaveChanges();

        }

        public void Disable(Membership membership)
        {
            var objFromDb = _db.Memberships.FirstOrDefault(c => c.Id == membership.Id);
            objFromDb.Status = false;
            _db.SaveChanges();

        }

        public IEnumerable<SelectListItem> GetMembershiptListForDropDown()
        {
            return _db.Memberships.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
    }
}
