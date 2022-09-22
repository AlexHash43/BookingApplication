using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.UserDtos
{
    public class ChangeUserRolesDto
    {
        public Guid Id { get; set; }
        /// <summary>
        ///     User new roles getter/setter.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}
