using GMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.DataAccess.Data.Repository.IRepository
{
    public interface IMemberTrainerRepository : IRepository<MemberTrainer>
    {
        IEnumerable<SelectListItem> GetMemberTrainerListForDropDown();
        void Update(MemberTrainer MemberTrainer);
    }
}
