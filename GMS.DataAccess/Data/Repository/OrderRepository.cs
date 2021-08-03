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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
       
        public void Update(Order order)
        {
            var objFromDb = _db.Orders.FirstOrDefault(c => c.Id == order.Id);
            objFromDb.ClientId = order.ClientId;
            objFromDb.MembershipId = order.MembershipId;
            objFromDb.ShiftId = order.ShiftId;
            objFromDb.StartDate= order.StartDate;
            objFromDb.EndDate = order.EndDate;
            objFromDb.PeriodId = order.PeriodId;
            objFromDb.Comments = order.Comments;
 
            _db.SaveChanges();

        }
       
    }
}
