using GMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.DataAccess.Data.Repository.IRepository
{
    public interface IDetailRepository
    {
        Trainer GetTrainerWithClients(int id);
        Equipment GetEquipmentWithMaintenances(int id);
        

    }
}
