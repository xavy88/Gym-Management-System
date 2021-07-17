using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Admin + "," + SD.Manager)]
    public class MembershipController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembershipController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert (int? id)
        {
            Membership membership = new Membership();
            if (id== null)
            {
                return View(membership);
            }
            membership = _unitOfWork.Membership.Get(id.GetValueOrDefault());
            if (membership == null)
            {
                return NotFound();
            }
            return View(membership);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
       public IActionResult Upsert(Membership membership)
        {
            if (ModelState.IsValid)
            {
                if (membership.Id ==0)
                {
                    _unitOfWork.Membership.Add(membership);
                }
                else
                {
                    _unitOfWork.Membership.Update(membership);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(membership);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Membership.GetAll() });
        }
      

        [HttpDelete]
        public IActionResult Delete (int id)
        {
            var objFromDb = _unitOfWork.Membership.Get(id);
            if (objFromDb==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Membership.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
        #endregion
    }
}
