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
    [Authorize(Roles = SD.Admin)]
    [Area("Admin")]
    public class TrainerScheduleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        [BindProperty]
        public TrainerScheduleVM trainerScheduleVM { get; set; }
 

        public TrainerScheduleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            return View();
        }

       

        public IActionResult Upsert(int? id)
        {
            trainerScheduleVM = new TrainerScheduleVM()
            {
                TrainerSchedule = new TrainerSchedule(),
                TrainerList = _unitOfWork.Trainer.GetTrainerListForDropDown(),
                PeriodList = _unitOfWork.Period.GetPeriodListForDropDown(),
                ShiftList = _unitOfWork.Shift.GetShiftListForDropDown(),
            };

            if (id != null)
            {
                trainerScheduleVM.TrainerSchedule = _unitOfWork.TrainerSchedule.Get(id.GetValueOrDefault());
            }

            return View(trainerScheduleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (trainerScheduleVM.TrainerSchedule.Id == 0)
                {
                    //Add Schedule
                    _unitOfWork.TrainerSchedule.Add(trainerScheduleVM.TrainerSchedule);
                }
                else
                {
                    //Edit Schedule
                    var trainerScheduleFromDb = _unitOfWork.TrainerSchedule.Get(trainerScheduleVM.TrainerSchedule.Id);
                    _unitOfWork.TrainerSchedule.Update(trainerScheduleVM.TrainerSchedule);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                trainerScheduleVM.TrainerList = _unitOfWork.Trainer.GetTrainerListForDropDown();
                trainerScheduleVM.PeriodList = _unitOfWork.Period.GetPeriodListForDropDown();
                trainerScheduleVM.ShiftList = _unitOfWork.Shift.GetShiftListForDropDown();
                return View(trainerScheduleVM);
            }
        }

        

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.TrainerSchedule.GetAll(includeProperties: "Trainer,Period,Shift") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var trainerScheduleFromDb = _unitOfWork.TrainerSchedule.Get(id);
                        
            if (trainerScheduleFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.TrainerSchedule.Remove(trainerScheduleFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion

    }
}
