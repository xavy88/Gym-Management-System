using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        [Display(Name = "Buy Price")]
        public double BuyPrice { get; set; }
        [Display(Name ="Buy Date")]
        [DataType(DataType.Date)]
        
        public DateTime BuyDate { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        public List<Maintenance> Maintenances { get; set; }

    }
}
