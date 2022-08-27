using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ProcedureForUpdateDto
    {
        //public Guid Id { get; set; }
        [Required(ErrorMessage = "Procedure name is required")]
        [StringLength(50, ErrorMessage = "Procedure name can't be longer than 50 characters"), Display(Name = "Procedure Name")]
        public string? ProcedureName { get; set; }
        //public ICollection<AppointmentDto>? AppointmentProcedures { get; set; }
    }
}
