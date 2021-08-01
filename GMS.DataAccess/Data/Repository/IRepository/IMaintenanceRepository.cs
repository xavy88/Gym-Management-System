using GMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.DataAccess.Data.Repository.IRepository
{
    public interface IMaintenanceRepository : IRepository<Maintenance>
    {
     //IEnumerable<SelectListItem> GetMaintenanceListForDropDown();
        void Update(Maintenance maintenance);
    }
}
