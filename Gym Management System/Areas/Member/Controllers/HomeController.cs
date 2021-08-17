using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using GMS.Models.ViewModel;
using Gym_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gym_Management_System.Controllers
{
    [Authorize]
    [Area("Member")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        [BindProperty]
        public ClientVM CliVM { get; set; }
        public OrderVM orderVM { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexClient()
        {
            return View();
        }

        public IActionResult OrderClient(int? id)
        {
            return View();
        }

        public IActionResult PlanClient(int? id)
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API Call
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.Name);

            return Json(new { data = _unitOfWork.Client.GetAll(t => t.Email == claims.Value, includeProperties: "Trainer") });
            
        }

       
        public IActionResult GetAllOrderClient()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.Name);

          
                return Json(new { data = _unitOfWork.Order.GetAll(o => o.Client.Email == claims.Value, includeProperties: "Client,Membership,Period,Shift") });
            
        }

        public IActionResult GetAllPlanClient()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.Name);

            return Json(new { data = _unitOfWork.Plan.GetAll(p => p.Client.Email == claims.Value, includeProperties: "Client") });
           

        }

        #endregion
    }
}
