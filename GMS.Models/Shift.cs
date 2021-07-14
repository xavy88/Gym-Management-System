using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models
{
    public class Shift
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name ="Start Time")]
        public string StartTime{ get; set; }
        [Display(Name = "End Time")]
        public string EndTime { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
