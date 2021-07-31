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
        IMembershipRepository Membership { get; }
        IEquipmentRepository Equipment { get; }
        IMemberRepository Member{ get; }
        IClientRepository Client { get; }
        ITrainerRepository Trainer { get; }
        IUserRepository User { get; }
        void Save();
    }
}
