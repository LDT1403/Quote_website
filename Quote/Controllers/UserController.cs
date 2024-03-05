﻿using AutoMapper.Internal;
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
    [Authorize]
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
        
        [HttpGet]
       
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                return Ok(users);
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
    }
}