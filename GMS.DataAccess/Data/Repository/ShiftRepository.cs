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
    public class ShiftRepository : Repository<Shift>, IShiftRepository
    {
        private readonly ApplicationDbContext _db;
        public ShiftRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetShiftListForDropDown()
        {
            return _db.Shift.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Shift shift)
        {
            var objFromDb = _db.Shift.FirstOrDefault(c => c.Id == shift.Id);
            objFromDb.Name = shift.Name;
            objFromDb.StartTime = shift.StartTime;
            objFromDb.EndTime = shift.EndTime;
            objFromDb.Description = shift.Description;

            _db.SaveChanges();

        }
    }
}
