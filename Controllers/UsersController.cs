using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly FirstAPIContext _context;
        private readonly IMapper _mapper;
        public UsersController(FirstAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var Users = await _context.Users.ToListAsync();

            return Ok(Users);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> getUserById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid User Id");
            }

            var user = await _context.Users.Include(b => b.Books).ThenInclude(b => b.Categories).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            if (string.IsNullOrWhiteSpace(user.Name))
            {
                return BadRequest("Name is required");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(getUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User Id mismatch");
            }

            var existingUser = _context.Users.Find(id);

            if (existingUser == null)
                return NotFound();

            existingUser.Name = user.Name;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return NoContent();


        }

    }
}
