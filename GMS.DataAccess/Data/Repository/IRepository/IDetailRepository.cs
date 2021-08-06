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
        //List<Client> GetClientWithTrainer(int id);

        Trainer GetTrainerWithClients(int id);
        Equipment GetEquipmentWithMaintenances(int id);

        //List<Trainer> GetAllTrainerWithClients();

    }
}
