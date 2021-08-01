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
    public class MaintenanceRepository : Repository<Maintenance>, IMaintenanceRepository
    {
        private readonly ApplicationDbContext _db;
        public MaintenanceRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        //public IEnumerable<SelectListItem> GetMaintenanceListForDropDown()
        //{
        //    return _db.Maintenances.Select(i => new SelectListItem()
        //    {
        //        Text = i.,
        //        Value = i.Id.ToString()
        //    });
        //}

        public void Update(Maintenance maintenance)
        {
            var objFromDb = _db.Maintenances.FirstOrDefault(c => c.Id == maintenance.Id);
            objFromDb.EquipmentId = maintenance.EquipmentId;
            objFromDb.Type = maintenance.Type;
            objFromDb.DateOfMaintenance = maintenance.DateOfMaintenance;
            objFromDb.Description= maintenance.Description;
            objFromDb.InvoiceUrl = maintenance.InvoiceUrl;
            objFromDb.Price = maintenance.Price;
       
            _db.SaveChanges();

        }
      
    }
}
