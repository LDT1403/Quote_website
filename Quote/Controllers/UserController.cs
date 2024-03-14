using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quote.Controllers
{
    [Route("api/[controller]")]
  
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserInterface _userService;

        private readonly IConfiguration _configuration;

        public UserController(UserInterface userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        
        [HttpGet("UserInfo")]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var users = await _userService.GetUserIDAsync(userId);
                var userInfo = new UserInfoModal
                {
                    UserName = users.UserName,
                    Email = users.Email,
                    Phone = users.Phone,
                    Position = users.Position,
                    Images = users.Image,
                    Date = users.Dob  
                };
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

     

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModal user)
        {
            try
            {
                var _user = _userService.GetUsersAsync().Result.SingleOrDefault(p => p.Email == user.Email && p.Password == user.Password);

                if (_user == null)
                {
                    return Ok(new ResModal
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    });
                }

                return Ok(new ResModal
                {
                    Success = true,
                    Message = "Success",
                    Data = GenerateToken(_user)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        private string GenerateToken(User user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim("Id", user.UserId.ToString()),
                new Claim("Email", user.Email),
                new Claim("UserName" ,user.UserName),
                new Claim("Role" ,user.Role),
                new Claim("ManageId" ,user.ManagerId.ToString()),
                new Claim(ClaimTypes.Role ,user.Role),
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterModal register)
        {
            try
            {
                var regis = await _userService.RegisterAsync(register);

                if (regis == null)
                {
                    return Ok(new ErrorRespon
                    {
                        Error = false,
                        Message = "Email đã tồn tại"
                    });
                }
                else
                {
                    return Ok(new ErrorRespon
                    {
                        Error = false,
                        Message = "Register Success"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("logout")]
     
        public async Task<IActionResult> Logout()
        {
            var userClaims = User.Claims.ToList();
            var tokenClaim = userClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            if (tokenClaim != null)
            {
                userClaims.Remove(tokenClaim);
            }
            return Ok();
        }

        [HttpGet("GetAllStaffById/{id}")]

        public async Task<IActionResult> GetAllStaffById(int id)
        {
            try
            {
                var list = await _userService.GetStaffById(id);


                List<StaffResponse> users = new List<StaffResponse>();

                foreach (var user in list)
                {
                    StaffResponse staff =new StaffResponse();
                     staff.UserName = user.UserName;
                    staff.UserId = user.UserId;
                    staff.UserName = user.UserName;
                    staff.Phone = user.Phone;
                    staff.Email = user.Email;
                    staff.Status =user.Status;
                    staff.Dob = user.Dob;
                    users.Add(staff);
                }
               
                return Ok(users);
               

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllStaffByStatus/{id}")]
        public async Task<IActionResult> GetAllStaffByStatus(int id)
        {
            try
            {
                var list = await _userService.GetStaffByStatus(id);

                List<StaffResponse> users = new List<StaffResponse>();

                foreach (var user in list)
                {
                    StaffResponse staff = new StaffResponse();
                    staff.UserName = user.UserName;
                    staff.UserId = user.UserId;
                    staff.UserName = user.UserName;
                    staff.Phone = user.Phone;
                    staff.Email = user.Email;
                    staff.Status = user.Status;
                    staff.Dob = user.Dob;
                    users.Add(staff);
                }

                return Ok(users);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
