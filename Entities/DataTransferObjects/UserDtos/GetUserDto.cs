using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.UserDtos
{
    public class GetUserDto
    {
        /// <summary>
        ///    Gets and sets the user UserId.
        /// </summary>
        public Guid Id { get; set; }
        public string FullName { get; set; }

        /// <summary>
        ///     Gets and sets the username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Gets and sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Gets and sets user's role.
        /// </summary>
        public IList<string> Roles { get; set; }
    }
}
