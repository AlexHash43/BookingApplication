using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ProcedureDto
    {
        public Guid Id { get; set; }
        public string? ProcedureName { get; set; }
    }
}
