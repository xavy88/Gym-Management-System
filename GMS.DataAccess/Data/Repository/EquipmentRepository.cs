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
    public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        private readonly ApplicationDbContext _db;
        public EquipmentRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetEquipmenttListForDropDown()
        {
            return _db.Equipment.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Equipment equipment)
        {
            var objFromDb = _db.Equipment.FirstOrDefault(c => c.Id == equipment.Id);
            objFromDb.Name = equipment.Name;
            objFromDb.Model = equipment.Model;
            objFromDb.Serial = equipment.Serial;
            objFromDb.BuyDate = equipment.BuyDate;
            objFromDb.BuyPrice = equipment.BuyPrice;
            objFromDb.Comments = equipment.Comments;
            objFromDb.Description = equipment.Description;
            

            _db.SaveChanges();

        }
    }
}
