using System.ComponentModel.DataAnnotations;

namespace FileTransfer_mvc.Dtos
{
    public class UserDto
    {
        public required string UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
