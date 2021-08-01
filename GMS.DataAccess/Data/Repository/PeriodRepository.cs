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
    public class PeriodRepository : Repository<Period>, IPeriodRepository
    {
        private readonly ApplicationDbContext _db;
        public PeriodRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetPeriodtListForDropDown()
        {
            return _db.Periods.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Period period)
        {
            var objFromDb = _db.Periods.FirstOrDefault(c => c.Id == period.Id);
            objFromDb.Name = period.Name;
            objFromDb.StartTime = period.StartTime;
            objFromDb.EndTime = period.EndTime;
            objFromDb.Description = period.Description;

            _db.SaveChanges();

        }
        public void Disable(Period period)
        {
            var objFromDb = _db.Periods.FirstOrDefault(c => c.Id == period.Id);
            objFromDb.Status = false;


            _db.SaveChanges();

        }
    }
}
