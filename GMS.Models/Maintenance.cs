using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        [Required]
        public int EquipmentId { get; set; }
        [ForeignKey("EquipmentId")]
        public Equipment Equipment { get; set; }
        [Required]
        public string Type { get; set; }
        [Display(Name = "Date Of Maintenance")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime DateOfMaintenance { get; set; }
        public string Description { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Invoice Image")]
        public string InvoiceUrl { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Price { get; set; }
        

    }
}
