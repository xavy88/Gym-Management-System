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
    public class TrainerRepository : Repository<Trainer>, ITrainerRepository
    {
        private readonly ApplicationDbContext _db;
        public TrainerRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetTrainerListForDropDown()
        {
            return _db.Trainer.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Trainer trainer)
        {
            var objFromDb = _db.Trainer.FirstOrDefault(c => c.Id == trainer.Id);
            objFromDb.Name = trainer.Name;
            objFromDb.Phone = trainer.Phone;
            objFromDb.Email = trainer.Email;
            objFromDb.Gender= trainer.Gender.ToUpper();
            objFromDb.Salary = trainer.Salary;
            objFromDb.DateOfBirth = trainer.DateOfBirth;
            objFromDb.Address = trainer.Address;
            objFromDb.ImageUrl = trainer.ImageUrl;
            objFromDb.Status= trainer.Status;


            _db.SaveChanges();

        }
        public void Disable(Trainer trainer)
        {
            var objFromDb = _db.Trainer.FirstOrDefault(c => c.Id == trainer.Id);
            objFromDb.Status = false;


            _db.SaveChanges();

        }
    }
}
