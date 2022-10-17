using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using API.DTOs;
using API.Interfaces;
namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) 
        {
            if(await UserExists(registerDto.Username))
                return BadRequest("Username is taken");
            using var hmac = new HMACSHA512();
            AppUser user = new AppUser {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            AppUser user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if(user == null) return Unauthorized("Invalid username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            byte[] password = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDto.Password));
            int l1 = user.PasswordHash.Length, l2 = password.Length;
            if(l1 != l2)
                return Unauthorized("Invalid password");
            else 
                for(int i = 0; i < l1; i++)
                    if(password[i] != user.PasswordHash[i])
                        return Unauthorized("Invalid password");
            return new UserDto {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}