using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class AppointmentForUpdateDto
    {
        public DateTime UpdatedOn { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
