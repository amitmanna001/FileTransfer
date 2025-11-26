using FileTransfer_mvc.Data;
using FileTransfer_mvc.Dtos;
using FileTransfer_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileTransfer_mvc.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _dbContext;
        private readonly IConfiguration _config;
        public UserController(ApplicationDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login([FromBody] UserDto userDto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userDto.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var checkPassword = Authenticate(userDto);
            if (!checkPassword)
            {
                return Unauthorized("Invalid credentials.");
            }
            var token = GenerateJwt(user);
            return Ok(token);
        }

        private string GenerateJwt(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured"));
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var token = new JwtSecurityToken(
                    _config["jwt:Issuer"],
                    _config["jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool Authenticate(UserDto userDto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userDto.Email);
            if (!PasswordHasher.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return false;
            }
            return true;
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult SubmitForm(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var findDublicateUser = _dbContext.Users.FirstOrDefault(u => u.Email == userDto.Email);
            if (findDublicateUser != null)
            {
                TempData["AlertMessage"] = "User email already exists.";
                return View("Register");
            }
            PasswordHasher.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var users = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.Now
            };
            _dbContext.Users.Add(users);
            _dbContext.SaveChanges();
            TempData["AlertMessage"] = "Successful insert record.";
            return View("Register");
        }
    }
}
