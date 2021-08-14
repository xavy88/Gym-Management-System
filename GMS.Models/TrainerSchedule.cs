using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models
{
    public class TrainerSchedule
    {
        public int Id { get; set; }
        [Required]
        public int TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }
        [Required]
        public int PeriodId { get; set; }
        [ForeignKey("PeriodId")]
        public Period Period { get; set; }
        [Required]
        public int ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string Comments { get; set; }
            
    }
}
