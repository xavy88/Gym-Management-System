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
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly ApplicationDbContext _db;
        public ClientRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetClientListForDropDown()
        {
            return _db.Clients.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Client client)
        {
            var objFromDb = _db.Clients.FirstOrDefault(c => c.Id == client.Id);
            objFromDb.Name = client.Name;
            objFromDb.Phone = client.Phone;
            objFromDb.Email = client.Email;
            objFromDb.Gender= client.Gender.ToUpper();
            objFromDb.Height = client.Height;
            objFromDb.Weight = client.Weight;
            objFromDb.DateOfBirth = client.DateOfBirth;
            objFromDb.Address = client.Address;
            objFromDb.ImageUrl = client.ImageUrl;
            objFromDb.TrainerId = client.TrainerId;
            objFromDb.Status= client.Status;


            _db.SaveChanges();

        }
        public void Disable(Client client)
        {
            var objFromDb = _db.Clients.FirstOrDefault(c => c.Id == client.Id);
            objFromDb.Status = false;


            _db.SaveChanges();

        }
    }
}
