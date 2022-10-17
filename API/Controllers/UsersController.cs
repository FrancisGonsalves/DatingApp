using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController: ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() 
        {
            return await _context.Users.ToListAsync();
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}