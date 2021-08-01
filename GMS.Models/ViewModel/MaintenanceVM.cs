using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models.ViewModel
{
    public class MaintenanceVM
    {
        public Maintenance Maintenance { get; set; }
        public IEnumerable<SelectListItem> EquipmentList { get; set; }
    }
}
