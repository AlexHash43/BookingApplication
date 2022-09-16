using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.UserDtos
{
    public class UserAuthDto
    {
        /// <summary>
        ///     Property for reading and writing usernames.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        ///     Poperty for reading and writing a user's name.
        /// </summary>
        public string Password { get; set; }
    }
}
