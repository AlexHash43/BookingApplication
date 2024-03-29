﻿using System;
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
        [Key]
        //[Column("ProcedureId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Procedure name is required")]
        [StringLength(50, ErrorMessage = "Procedure name can't be longer than 50 characters"), Display(Name = "Procedure Name")]
        public string? ProcedureName { get; set; }
        [InverseProperty(nameof(Appointment.AppointmentProcedure))]
        public virtual ICollection<Appointment>? AppointmentProcedures { get; set; }
    }
}
