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
            Period = new PeriodRepository(_db);
            Membership = new MembershipRepository(_db);
            Equipment = new EquipmentRepository(_db);
            Member = new MemberRepository(_db);
            Client = new ClientRepository(_db);
            Maintenance = new MaintenanceRepository(_db);
            Trainer = new TrainerRepository(_db);
            User = new UserRepository(_db);
            Order = new OrderRepository(_db);
            Plan = new PlanRepository(_db);
            TrainerSchedule = new TrainerScheduleRepository(_db);
            SP_Call = new SP_Call(db);


        }
        public IShiftRepository Shift { get; private set; }
        public IPeriodRepository Period { get; private set; }
        public IMembershipRepository Membership { get; private set; }
        public IEquipmentRepository Equipment { get; private set; }
        public IMemberRepository Member { get; private set; }
        public IClientRepository Client { get; private set; }
        public IMaintenanceRepository Maintenance { get; private set; }
        public ITrainerRepository Trainer { get; private set; }
        public IUserRepository User { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IDetailRepository Detail { get; private set; }
        public IPlanRepository Plan { get; private set; }
        public ITrainerScheduleRepository TrainerSchedule { get; private set; }
        public ISP_Call SP_Call { get; private set; }

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
