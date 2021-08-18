using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using GMS.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gym_Management_System.Areas.Admin.Controllers
{
    //[Authorize(Roles = SD.Admin + "," + SD.Manager)]
    [Area("Admin")]
    public class TrainerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public Trainer trainer { get; set; }
        public TrainerController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexTrainerOfClient()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            trainer = new Trainer();
            if (id!=null)
            {
                trainer = _unitOfWork.Trainer.Get(id.GetValueOrDefault());
            }
            return View(trainer);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (trainer.Id == 0)
                {
                    //new Equipment
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\trainer");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads,fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    trainer.ImageUrl = @"\images\trainer\" + fileName + extension;

                    _unitOfWork.Trainer.Add(trainer);

                }
                else
                {
                    //Edit Trainer

                    var trainerFromDb = _unitOfWork.Trainer.Get(trainer.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\trainer");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, trainerFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        trainer.ImageUrl = @"\images\trainer\" + fileName + extension_new;
                    }
                    else
                    {
                        trainer.ImageUrl = trainerFromDb.ImageUrl;
                    }

                    _unitOfWork.Trainer.Update(trainer);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(trainer);
            }
        }

       
        #region API Calls
        public IActionResult GetAll()
        {

            //var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            //var claims = claimsIdentity.FindFirst(ClaimTypes.Name);

            //return Json(new { data = _unitOfWork.Trainer.GetAll(t => t.Email == claims.Value) });
           return Json(new { data = _unitOfWork.SP_Call.ReturnList<Trainer>(SD.usp_GetAllTrainers, null) });

        }

        public IActionResult GetAllClientOfClient()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.Name);

            return Json(new { data = _unitOfWork.Client.GetAll(t => t.Trainer.Email == claims.Value, includeProperties: "Trainer") });

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var trainerFromDb = _unitOfWork.Trainer.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, trainerFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (trainerFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Trainer.Remove(trainerFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion
    }
}
