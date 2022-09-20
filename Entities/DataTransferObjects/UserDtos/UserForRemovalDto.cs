using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.UserDtos
{
    public class UserForRemovalDto
    {
        /// <summary>
        ///     Return user model list getter and setter
        /// </summary>
       // public IEnumerable<GetUserDto> Users { get; set; }

        public GetUserDto User { get; set; }
        /// <summary>
        ///     Return message getter and setter.
        /// </summary>
        public string Message { get; set; }
    }
}
