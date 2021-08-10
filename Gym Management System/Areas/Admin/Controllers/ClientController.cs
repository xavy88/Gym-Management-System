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
    public class ClientController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public ClientVM CliVM { get; set; }

        public ClientController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
            CliVM = new ClientVM()
            {
                Client = new Client(),
                TrainerList = _unitOfWork.Trainer.GetTrainerListForDropDown(),
            };

            if (id != null)
            {
                CliVM.Client = _unitOfWork.Client.Get(id.GetValueOrDefault());
            }

            return View(CliVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (CliVM.Client.Id == 0)
                {
                    //New Client
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\client");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    CliVM.Client.ImageUrl = @"\images\client\" + fileName + extension;

                    _unitOfWork.Client.Add(CliVM.Client);
                }
                else
                {
                    //Edit Client
                    var clientFromDb = _unitOfWork.Client.Get(CliVM.Client.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\client");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, clientFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        CliVM.Client.ImageUrl = @"\images\client\" + fileName + extension_new;
                    }
                    else
                    {
                        CliVM.Client.ImageUrl = clientFromDb.ImageUrl;
                    }

                    _unitOfWork.Client.Update(CliVM.Client);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                CliVM.TrainerList = _unitOfWork.Trainer.GetTrainerListForDropDown();
                return View(CliVM);
            }
        }

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Client.GetAll(includeProperties: "Trainer") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var clientFromDb = _unitOfWork.Client.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, clientFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (clientFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Client.Remove(clientFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion

    }
}
