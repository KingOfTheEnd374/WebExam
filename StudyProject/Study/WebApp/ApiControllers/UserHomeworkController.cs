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
    public class UserHomeworkController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.UserHomework, App.Domain.UserHomework> _mapper;

        public UserHomeworkController(AppDbContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.UserHomework, App.Domain.UserHomework>(autoMapper);
        }

        // GET: api/UserHomework
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserHomework>>> GetUserHomeworks()
        {
            return Ok((await _context.UserHomeworks.ToListAsync()).Select(e => _mapper.Map(e)));
        }

        // GET: api/UserHomework/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserHomework>> GetUserHomework(Guid id)
        {
            var userHomework = _mapper.Map(await _context.UserHomeworks.FindAsync(id));

            if (userHomework == null)
            {
                return NotFound();
            }

            return userHomework;
        }

        // PUT: api/UserHomework/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserHomework(Guid id, UserHomework userHomework)
        {
            if (id != userHomework.Id)
            {
                return BadRequest();
            }

            _context.Entry(userHomework).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserHomeworkExists(id))
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

        // POST: api/UserHomework
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserHomework>> PostUserHomework(UserHomework userHomework)
        {
            _context.UserHomeworks.Add(_mapper.Map(userHomework));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserHomework", new { id = userHomework.Id }, userHomework);
        }

        // DELETE: api/UserHomework/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserHomework(Guid id)
        {
            var userHomework = await _context.UserHomeworks.FindAsync(id);
            if (userHomework == null)
            {
                return NotFound();
            }

            _context.UserHomeworks.Remove(userHomework);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserHomeworkExists(Guid id)
        {
            return _context.UserHomeworks.Any(e => e.Id == id);
        }
        
        // GET: api/Subject/5
        [HttpGet("user:{userId}/hw:{hwId}")]
        public async Task<ActionResult<IEnumerable<UserHomework>>> GetGrade(Guid userId, Guid hwId)
        {
            var hwResult = await _context.UserHomeworks
                .FirstAsync(e => e.AppUserId == userId && e.HomeworkId == hwId);

            if (hwResult == null)
            {
                return NotFound();
            }

            return Ok(hwResult);
        }
        
        // POST: api/UserSubject
        [HttpPost("user:{userId}/hw:{homeworkId}/grade:{grade}")]
        public async Task<ActionResult> SetGrade(Guid userId, Guid homeworkId, int grade)
        {
            App.Domain.UserHomework? existing = null;
            try
            {
                existing = await _context.UserHomeworks.FirstAsync(u => u.AppUserId == userId && u.HomeworkId == homeworkId);
            }
            catch (Exception e)
            {

            }

            if (existing != null)
            {
                _context.UserHomeworks.Remove(existing);
                await _context.SaveChangesAsync();
            }
            
            var userGrade = new UserHomework();
            userGrade.AppUserId = userId;
            userGrade.HomeworkId = homeworkId;
            userGrade.Grade = grade;
                
            _context.UserHomeworks.Add(_mapper.Map(userGrade));
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
