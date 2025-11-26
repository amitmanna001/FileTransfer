using System.ComponentModel.DataAnnotations;

namespace FileTransfer_mvc.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
