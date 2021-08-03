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
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        [BindProperty]
        public OrderVM orderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            orderVM = new OrderVM()
            {
                Order = new Order(),
                ClientList = _unitOfWork.Client.GetClientListForDropDown(),
                MembershipList = _unitOfWork.Membership.GetMembershipListForDropDown(),
                PeriodList = _unitOfWork.Period.GetPeriodListForDropDown(),
                ShiftList = _unitOfWork.Shift.GetShiftListForDropDown(),
            };

            if (id != null)
            {
                orderVM.Order = _unitOfWork.Order.Get(id.GetValueOrDefault());
            }

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (orderVM.Order.Id == 0)
                {
                    //Add Order
                    _unitOfWork.Order.Add(orderVM.Order);
                }
                else
                {
                    //Edit Order
                    var orderFromDb = _unitOfWork.Order.Get(orderVM.Order.Id);
                    _unitOfWork.Order.Update(orderVM.Order);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                orderVM.ClientList = _unitOfWork.Client.GetClientListForDropDown();
                orderVM.MembershipList = _unitOfWork.Membership.GetMembershipListForDropDown();
                orderVM.PeriodList = _unitOfWork.Period.GetPeriodListForDropDown();
                orderVM.ShiftList = _unitOfWork.Shift.GetShiftListForDropDown();
                return View(orderVM);
            }
        }

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Order.GetAll(includeProperties: "Client,Membership,Period,Shift") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var orderFromDb = _unitOfWork.Order.Get(id);
                        
            if (orderFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Order.Remove(orderFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion

    }
}
