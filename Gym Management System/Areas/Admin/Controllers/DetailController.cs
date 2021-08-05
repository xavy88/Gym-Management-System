using GMS.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Management_System.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DetailController : Controller
    {
        private readonly IDetailRepository _detailRepo;

        public DetailController(IDetailRepository detailRepo)
        {
            _detailRepo = detailRepo;
        }
        // GET: Trainer/Details/5
        public IActionResult TrainerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = _detailRepo.GetTrainerWithClients(id.GetValueOrDefault());
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }
    }
}
