using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.DTO.v1_0;
using Asp.Versioning;
using AutoMapper;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserSemesterController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.UserSemester, App.Domain.UserSemester> _mapper;

        public UserSemesterController(AppDbContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.UserSemester, App.Domain.UserSemester>(autoMapper);
        }

        // GET: api/UserSemester
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSemester>>> GetUserSemester()
        {
            return Ok((await _context.UserSemester.ToListAsync()).Select(e => _mapper.Map(e)));
        }

        // GET: api/UserSemester/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSemester>> GetUserSemester(Guid id)
        {
            var userSemester = _mapper.Map(await _context.UserSemester.FindAsync(id));

            if (userSemester == null)
            {
                return NotFound();
            }

            return userSemester;
        }

        // PUT: api/UserSemester/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserSemester(Guid id, UserSemester userSemester)
        {
            if (id != userSemester.Id)
            {
                return BadRequest();
            }

            _context.Entry(userSemester).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserSemesterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserSemester
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserSemester>> PostUserSemester(UserSemester userSemester)
        {
            _context.UserSemester.Add(_mapper.Map(userSemester));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserSemester", new { id = userSemester.Id }, userSemester);
        }

        // DELETE: api/UserSemester/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSemester(Guid id)
        {
            var userSemester = await _context.UserSemester.FindAsync(id);
            if (userSemester == null)
            {
                return NotFound();
            }

            _context.UserSemester.Remove(userSemester);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserSemesterExists(Guid id)
        {
            return _context.UserSemester.Any(e => e.Id == id);
        }
    }
}
