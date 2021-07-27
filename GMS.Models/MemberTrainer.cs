using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS.Models
{
    public class MemberTrainer
    {
        public int Id { get; set; }
        [ForeignKey("Member")]
        public int Member_Id { get; set; }
        [ForeignKey("Trainer")]
        public int Trainer_Id { get; set; }

        public Member Member { get; set; }
        public Trainer Trainer { get; set; }
    }
}
