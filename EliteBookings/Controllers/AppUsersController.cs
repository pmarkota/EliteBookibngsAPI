using AutoMapper;
using EliteBookings.DTOModels.AppUser;
using EliteBookings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EliteBookings.Controllers
{

    public class AppUserRegister
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class AppUserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly postgresContext _db;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AppUsersController(postgresContext db, IConfiguration config, IMapper mapper)
        {
            _db = db;
            _config = config;
            _mapper = mapper;
        }

        // GET: api/AppUsers - get all users and their reservation count
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOAppUser>>> GetAppUsers()
        {
            var users = await _db.AppUsers.ToListAsync();
            var dtoUsers = _mapper.Map<IEnumerable<AppUser>, IEnumerable<DTOAppUser>>(users);

            foreach (var user in dtoUsers)
            {
                user.ReservationCount = await _db.Reservations.Where(r => r.UserId == user.UserId).CountAsync();
            }

            return Ok(dtoUsers);
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(AppUserRegister user)
        {
            var newUser = new AppUser
            {
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Role = "user",
                IsConfirmed = false,
            };

            if (_db.AppUsers.Any(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists");
            }
            else if (_db.AppUsers.Any(u => u.Email == user.Email))
            {
                return BadRequest("Email already exists");
            }
            else
            {
                _db.AppUsers.Add(newUser);
            }
            await _db.SaveChangesAsync();

            return Ok(newUser);
        }

        //login and return jwt token
        [HttpPost("login")]
        public async Task<IActionResult> Login(AppUserLogin user)
        {
            var foundUser = await _db.AppUsers.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (foundUser == null)
            {
                return NotFound();
            }

            if (BCrypt.Net.BCrypt.Verify(user.Password, foundUser.PasswordHash))
            {
                var claims = new[]
                {
                    new Claim("id", foundUser.UserId.ToString()),
                    new Claim("username", foundUser.Username),
                    new Claim("role", foundUser.Role),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key")));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            return Unauthorized();
        }
    }
}