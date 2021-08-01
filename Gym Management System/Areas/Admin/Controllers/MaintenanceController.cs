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
    public class MaintenanceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public MaintenanceVM maintVM { get; set; }

        public MaintenanceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            maintVM = new MaintenanceVM()
            {
                Maintenance = new Maintenance(),
                EquipmentList = _unitOfWork.Equipment.GetEquipmentListForDropDown(),
            };

            if (id != null)
            {
                maintVM.Maintenance = _unitOfWork.Maintenance.Get(id.GetValueOrDefault());
            }

            return View(maintVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (maintVM.Maintenance.Id == 0)
                {
                    //New Maintenance
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\maintenance");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    maintVM.Maintenance.InvoiceUrl = @"\images\maintenance\" + fileName + extension;

                    _unitOfWork.Maintenance.Add(maintVM.Maintenance);
                }
                else
                {
                    //Edit Maintenance
                    var maintFromDb = _unitOfWork.Maintenance.Get(maintVM.Maintenance.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\client");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, maintFromDb.InvoiceUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        maintVM.Maintenance.InvoiceUrl = @"\images\maintenance\" + fileName + extension_new;
                    }
                    else
                    {
                        maintVM.Maintenance.InvoiceUrl = maintFromDb.InvoiceUrl;
                    }

                    _unitOfWork.Maintenance.Update(maintVM.Maintenance);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                maintVM.EquipmentList = _unitOfWork.Equipment.GetEquipmentListForDropDown();
                return View(maintVM);
            }
        }

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Maintenance.GetAll(includeProperties: "Equipment") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var maintFromDb = _unitOfWork.Maintenance.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, maintFromDb.InvoiceUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (maintFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Maintenance.Remove(maintFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion

    }
}
