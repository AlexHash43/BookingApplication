using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Procedure
    {
        [Required, Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProcId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Procedure name can't be longer than 50 characters"), Display(Name = "Procedure Name")]
        public string ProcedureName { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
