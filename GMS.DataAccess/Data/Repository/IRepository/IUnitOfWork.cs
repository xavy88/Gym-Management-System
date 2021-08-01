using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IShiftRepository Shift { get; }
        IPeriodRepository Period { get; }
        IMembershipRepository Membership { get; }
        IEquipmentRepository Equipment { get; }
        IMemberRepository Member{ get; }
        IClientRepository Client { get; }
        IMaintenanceRepository Maintenance { get; }
        ITrainerRepository Trainer { get; }
        IUserRepository User { get; }
        void Save();
    }
}
