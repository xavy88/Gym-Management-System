using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using Gym_Management_System.Data;
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
    //[Authorize(Roles = SD.Admin + "," + SD.Manager)]
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Member member { get; set; }
        public MemberController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, ApplicationDbContext db)
        {
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            member = new Member();
            if (id!=null)
            {
                member = _unitOfWork.Member.Get(id.GetValueOrDefault());
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
                if (member.Id == 0)
                {
                    //new Equipment
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\member");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads,fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    member.ImageUrl = @"\images\member\" + fileName + extension;

                    _unitOfWork.Member.Add(member);

                }
                else
                {
                    //Edit Members

                    var memberFromDb = _unitOfWork.Member.Get(member.Id);
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

                        member.ImageUrl = @"\images\member\" + fileName + extension_new;
                    }
                    else
                    {
                        member.ImageUrl = memberFromDb.ImageUrl;
                    }

                    _unitOfWork.Member.Update(member);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(member);
            }
        }

        #region Manage Trainer
        public IActionResult ManageTrainers(int id)
        {
            MemberTrainerVM obj = new MemberTrainerVM
            {
                MemberTrainerList = _db.MemberTrainers.Include(u => u.Trainer).Include(u => u.Member).Where(u => u.Member_Id == id).ToList(),
                MemberTrainer = new MemberTrainer()
                {
                    Member_Id = id
                },
                Member = _db.Members.FirstOrDefault(u => u.Id == id)
            };
            List<int> tempListOfAssignedTrainers = obj.MemberTrainerList.Select(u => u.Trainer_Id).ToList();
            //Not In Clause in LINQ
            //get all the trainers who id is not in tempListOfAssignedTrainer
            var tempList = _db.Trainer.Where(u => !tempListOfAssignedTrainers.Contains(u.Id)).ToList();

            obj.TrainerList = tempList.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(obj);
        }
        [HttpPost]
        public IActionResult ManageTrainers(MemberTrainerVM memberTrainerVM)
        {
            if (memberTrainerVM.MemberTrainer.Member_Id != 0 /*&& memberTrainerVM.MemberTrainer.Member_Id = 0*/)
            {
                _db.MemberTrainers.Add(memberTrainerVM.MemberTrainer);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(ManageTrainers), new { @id = memberTrainerVM.MemberTrainer.Member_Id });
        }

        [HttpPost]
        public IActionResult RemoveTrainers(int trainerId, MemberTrainerVM memberTrainerVM)
        {
            int memberId = memberTrainerVM.Member.Id;
            MemberTrainer memberTrainer = _db.MemberTrainers.FirstOrDefault(
                u => u.Member_Id == memberId && u.Trainer_Id == trainerId);

            _db.MemberTrainers.Remove(memberTrainer);
            _db.SaveChanges();


            return RedirectToAction(nameof(ManageTrainers), new { @id = trainerId });
        }
        #endregion

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Member.GetAll()});
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
