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
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        private readonly ApplicationDbContext _db;
        public MemberRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetMemberListForDropDown()
        {
            return _db.Equipment.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Member member)
        {
            var objFromDb = _db.Members.FirstOrDefault(c => c.Id == member.Id);
            objFromDb.Name = member.Name;
            objFromDb.Phone = member.Phone;
            objFromDb.Email = member.Email;
            objFromDb.Gender= member.Gender.ToUpper();
            objFromDb.Height = member.Height;
            objFromDb.Weight = member.Weight;
            objFromDb.DateOfBirth = member.DateOfBirth;
            objFromDb.Address = member.Address;
            objFromDb.ImageUrl = member.ImageUrl;
            objFromDb.Status= member.Status;


            _db.SaveChanges();

        }
        public void Disable(Member member)
        {
            var objFromDb = _db.Members.FirstOrDefault(c => c.Id == member.Id);
            objFromDb.Status = false;


            _db.SaveChanges();

        }
    }
}
