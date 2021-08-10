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
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        private readonly ApplicationDbContext _db;
        public PlanRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetPlanListForDropDown()
        {
            return _db.Plans.Select(i => new SelectListItem()
            {
                Text = i.Reference,
                Value = i.Id.ToString()
            });
        }

        public void Update(Plan plan)
        {
            var objFromDb = _db.Plans.FirstOrDefault(c => c.Id == plan.Id);
            objFromDb.Reference = plan.Reference;
            objFromDb.StartDate = plan.StartDate;
            objFromDb.EndDate = plan.EndDate;
            objFromDb.Monday= plan.Monday;
            objFromDb.Tuesday = plan.Tuesday;
            objFromDb.Wednesday = plan.Wednesday;
            objFromDb.Thursday = plan.Thursday;
            objFromDb.Friday = plan.Friday;
            objFromDb.Saturday = plan.Saturday;
            objFromDb.ClientId = plan.ClientId;
            
            _db.SaveChanges();

        }
       
    }
}
