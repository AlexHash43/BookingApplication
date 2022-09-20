using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.UserDtos
{
    public class ChangeUserDto
    {
        public Guid Id { get; set; }

        /// <summary>
        ///     User new first name getter/setter.
        /// </summary>
        [RegularExpression(@"^[^\s.!?\\_\/]+$", ErrorMessage = "First Name should not include whitespace or any special character")]
        public string FirstName { get; set; }

        /// <summary>
        ///     User new last name getter/setter.
        /// </summary>
        [RegularExpression(@"^[^\s.!?\\_\/]+$", ErrorMessage = "Last Name should not include whitespace or any special character")]
        public string LastName { get; set; }

        /// <summary>
        ///     User new email getter/setter.
        /// </summary>
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Your Email is not in the right format")]
        public string Email { get; set; }

        /// <summary>
        ///     User new username getter/setter.
        /// </summary>
        [RegularExpression(@"^[^\s.!?\\\/]+$", ErrorMessage = "User Name should not include whitespace or any special character")]
        public string UserName { get; set; }

        /// <summary>
        ///     User new roles getter/setter.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}
