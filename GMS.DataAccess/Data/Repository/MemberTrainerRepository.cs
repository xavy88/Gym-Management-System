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
    public class MemberTrainerRepository : Repository<MemberTrainer>, IMemberTrainerRepository
    {
        private readonly ApplicationDbContext _db;
        public MemberTrainerRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetMemberTrainerListForDropDown()
        {
            return _db.MemberTrainers.Select(i => new SelectListItem()
            {
                Text = i.Trainer.Name,
                Value = i.Trainer.Id.ToString()
            });
        }

        public void Update(MemberTrainer memberTrainer)
        {
            //var objFromDb = _db.MemberTrainers.FirstOrDefault(c => c.Trainer.Id == memberTrainer.Trainer.Id);
            //objFromDb.Trainer.Name = member.Name;
            //objFromDb.Phone = member.Phone;
            //objFromDb.Email = member.Email;
            //objFromDb.Gender= member.Gender.ToUpper();
            //objFromDb.Height = member.Height;
            //objFromDb.Weight = member.Weight;
            //objFromDb.DateOfBirth = member.DateOfBirth;
            //objFromDb.Address = member.Address;
            //objFromDb.ImageUrl = member.ImageUrl;
            //objFromDb.Status= member.Status;


            //_db.SaveChanges();

        }
        
    }
}
