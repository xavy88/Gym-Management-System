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
    [Authorize(Roles = SD.Admin)]
    public class ShiftController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShiftController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert (int? id)
        {
            Shift shift = new Shift();
            if (id== null)
            {
                return View(shift);
            }
            shift = _unitOfWork.Shift.Get(id.GetValueOrDefault());
            if (shift == null)
            {
                return NotFound();
            }
            return View(shift);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
       public IActionResult Upsert(Shift shift)
        {
            if (ModelState.IsValid)
            {
                if (shift.Id ==0)
                {
                    _unitOfWork.Shift.Add(shift);
                }
                else
                {
                    _unitOfWork.Shift.Update(shift);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(shift);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Shift.GetAll() });
        }
      

        [HttpDelete]
        public IActionResult Delete (int id)
        {
            var objFromDb = _unitOfWork.Shift.Get(id);
            if (objFromDb==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Shift.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
        #endregion
    }
}
