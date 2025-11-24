using FileTransfer_mvc.Data;
using FileTransfer_mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileTransfer_mvc.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _dbContext;
        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult SubmitForm(User user)
        {
            if (ModelState.IsValid)
            {
                var users = new User
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.Password,
                    CreatedAt = DateTime.Now
                };
                _dbContext.Users.Add(users);
                _dbContext.SaveChanges();
            }
            return View("Register");
        }
    }
}
