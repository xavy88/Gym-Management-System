using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using GMS.Models.ViewModel;
using GMS.Utility;
using Gym_Management_System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Management_System.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Admin)]
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
   
        [BindProperty]
        public MemberVM member { get; set; }
        public MemberController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
            member = new MemberVM()
            {
                Member = new Member(),
                TrainerList = _unitOfWork.Trainer.GetTrainerListForDropDown(),
            };
            if (id != null)
            {
                member.Member = _unitOfWork.Member.Get(id.GetValueOrDefault());
            }
            return View(member);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (member.Member.Id == 0)
                {
                    //new Member
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\member");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    member.Member.ImageUrl = @"\images\member\" + fileName + extension;

                    _unitOfWork.Member.Add(member.Member);

                }
                else
                {
                    //Edit Members

                    var memberFromDb = _unitOfWork.Member.Get(member.Member.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\member");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, memberFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        member.Member.ImageUrl = @"\images\member\" + fileName + extension_new;
                    }
                    else
                    {
                        member.Member.ImageUrl = memberFromDb.ImageUrl;
                    }

                    _unitOfWork.Member.Update(member.Member);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                member.TrainerList = _unitOfWork.Trainer.GetTrainerListForDropDown();
                return View(member);
            }
        }
               
        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Member.GetAll(includeProperties:"Trainer")});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var memberFromDb = _unitOfWork.Member.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, memberFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (memberFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Member.Remove(memberFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion
    }
}
