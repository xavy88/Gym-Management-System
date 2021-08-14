using GMS.DataAccess.Data.Repository.IRepository;
using GMS.Models;
using Gym_Management_System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.DataAccess.Data.Repository
{
    public class TrainerScheduleRepository : Repository<TrainerSchedule>, ITrainerScheduleRepository
    {
        private readonly ApplicationDbContext _db;
        public TrainerScheduleRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
       
        public void Update(TrainerSchedule trainerSchedule)
        {
            var objFromDb = _db.TrainerSchedule.FirstOrDefault(c => c.Id == trainerSchedule.Id);
            objFromDb.TrainerId = trainerSchedule.TrainerId;
            objFromDb.PeriodId = trainerSchedule.PeriodId;
            objFromDb.ShiftId = trainerSchedule.ShiftId;
            objFromDb.StartDate= trainerSchedule.StartDate;
            objFromDb.EndDate = trainerSchedule.EndDate;
            objFromDb.Comments = trainerSchedule.Comments;
 
            _db.SaveChanges();

        }
       
    }
}
