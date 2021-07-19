using GMS.DataAccess.Data.Repository.IRepository;
using Gym_Management_System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.DataAccess.Data.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Shift = new ShiftRepository(_db);
            Membership = new MembershipRepository(_db);
            Equipment = new EquipmentRepository(_db);
            Member = new MemberRepository(_db);
        }
        public IShiftRepository Shift { get; private set; }
        public IMembershipRepository Membership { get; private set; }
        public IEquipmentRepository Equipment { get; private set; }
        public IMemberRepository Member { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
