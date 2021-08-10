using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models.ViewModel
{
    public class PlanVM
    {
        public Plan Plan { get; set; }
        public IEnumerable<SelectListItem> ClientList { get; set; }
    }
}
