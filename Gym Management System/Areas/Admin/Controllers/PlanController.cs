using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models.ViewModel;
using GMS.Models;
using GMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Gym_Management_System.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Admin + "," + SD.Trainer)]
    [Area("Admin")]
    public class PlanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        [BindProperty]
        public PlanVM planVM { get; set; }

        public PlanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            return View();
        }
               
        public IActionResult Upsert(int? id)
        {
            planVM = new PlanVM()
            {
                Plan = new Plan(),
                ClientList = _unitOfWork.Client.GetClientListForDropDown(),
            };

            if (id != null)
            {
                planVM.Plan = _unitOfWork.Plan.Get(id.GetValueOrDefault());
            }

            return View(planVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
               
                if (planVM.Plan.Id == 0)
                {
                    //New Plan
                    _unitOfWork.Plan.Add(planVM.Plan);
                }
                else
                {
                    //Edit Plan
                    var planFromDb = _unitOfWork.Plan.Get(planVM.Plan.Id);
                    _unitOfWork.Plan.Update(planVM.Plan);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                planVM.ClientList = _unitOfWork.Client.GetClientListForDropDown();
                return View(planVM);
            }
        }

        #region API Calls
        public IActionResult GetAll()
        {
            var date = DateTime.Today;
            return Json(new { data = _unitOfWork.Plan.GetAll(p=>p.EndDate>=date,includeProperties: "Client") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var planFromDb = _unitOfWork.Plan.Get(id);
            
            if (planFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Plan.Remove(planFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion

    }
}
