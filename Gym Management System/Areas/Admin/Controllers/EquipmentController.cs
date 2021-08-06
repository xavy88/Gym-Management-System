using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using GMS.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Management_System.Areas.Admin.Controllers
{
    //[Authorize(Roles = SD.Admin + "," + SD.Manager)]
    [Area("Admin")]
    public class EquipmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public Equipment equipment { get; set; }
        public EquipmentController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            equipment = new Equipment();
            if (id!=null)
            {
                equipment = _unitOfWork.Equipment.Get(id.GetValueOrDefault());
            }
            return View(equipment);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (equipment.Id == 0)
                {
                    //new Equipment
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\equipment");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads,fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    equipment.ImageUrl = @"\images\equipment\" + fileName + extension;

                    _unitOfWork.Equipment.Add(equipment);

                }
                else
                {
                    //Edit Equipment

                    var equipmentFromDb = _unitOfWork.Equipment.Get(equipment.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\equipment");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, equipmentFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        equipment.ImageUrl = @"\images\equipment\" + fileName + extension_new;
                    }
                    else
                    {
                        equipment.ImageUrl = equipmentFromDb.ImageUrl;
                    }

                    _unitOfWork.Equipment.Update(equipment);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(equipment);
            }
        }

        #region API Calls
        public IActionResult GetAll()
        {
            //return Json(new { data = _unitOfWork.Equipment.GetAll()});
            return Json(new { data = _unitOfWork.SP_Call.ReturnList<Equipment>(SD.usp_GetAllEquipments, null) });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var equipmentFromDb = _unitOfWork.Equipment.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, equipmentFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (equipmentFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Equipment.Remove(equipmentFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion
    }
}
