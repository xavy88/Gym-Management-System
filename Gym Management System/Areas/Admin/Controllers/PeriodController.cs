using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using GMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin + "," + SD.Trainer)]
    public class PeriodController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PeriodController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert (int? id)
        {
            Period period = new Period();
            if (id== null)
            {
                return View(period);
            }
            period = _unitOfWork.Period.Get(id.GetValueOrDefault());
            if (period == null)
            {
                return NotFound();
            }
            return View(period);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
       public IActionResult Upsert(Period period)
        {
            if (ModelState.IsValid)
            {
                if (period.Id ==0)
                {
                    _unitOfWork.Period.Add(period);
                }
                else
                {
                    _unitOfWork.Period.Update(period);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(period);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Period.GetAll() });
        }
      

        [HttpDelete]
        public IActionResult Delete (int id)
        {
            var objFromDb = _unitOfWork.Period.Get(id);
            if (objFromDb==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Period.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
        #endregion
    }
}
