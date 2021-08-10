using GMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.DataAccess.Data.Repository.IRepository
{
    public interface IPlanRepository : IRepository<Plan>
    {
        IEnumerable<SelectListItem> GetPlanListForDropDown();
        void Update(Plan Plan);
    }
}
