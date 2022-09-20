using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.UserDtos
{
    public class UserForCreationDto
    {
        /// <summary>
        ///     Gets and sets user id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets and sets user fist name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Gets and sets user last name.
        /// </summary>
        public string LastName { get; set; }
        public string FullName { get; set; }

        /// <summary>
        ///     Gets and sets user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Gets and sets user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Gets and sets user roles.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}
