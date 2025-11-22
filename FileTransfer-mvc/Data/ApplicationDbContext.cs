using System.Data;
using FileTransfer_mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace FileTransfer_mvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
