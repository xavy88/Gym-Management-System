using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models.ViewModel
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public IEnumerable<SelectListItem> ClientList { get; set; }
        public IEnumerable<SelectListItem> MembershipList { get; set; }
        public IEnumerable<SelectListItem> PeriodList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
    }
}
