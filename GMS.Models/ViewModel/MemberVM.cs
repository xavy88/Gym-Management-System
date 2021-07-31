using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models.ViewModel
{
    public class MemberVM
    {
        public Member Member { get; set; }
        public IEnumerable<SelectListItem> TrainerList { get; set; }
    }
}
