using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.UserDtos
{
    public class GetUserDetailsDto
    {
        /// <summary>
        ///    Gets and sets the user UserId.
        /// </summary>
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        /// <summary>
        ///     Gets and sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Gets and sets the username.
        /// </summary>
        public string UserName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }


        /// <summary>
        ///     Gets and sets user's role.
        /// </summary>
        public IList<string> Roles { get; set; }
    }
}
