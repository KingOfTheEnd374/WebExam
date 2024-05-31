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
    public class UserSubjectController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.UserSubject, App.Domain.UserSubject> _mapper;

        public UserSubjectController(AppDbContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.UserSubject, App.Domain.UserSubject>(autoMapper);
        }

        // GET: api/UserSubject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSubject>>> GetUserSubjects()
        {
            return Ok((await _context.UserSubjects.ToListAsync()).Select(e => _mapper.Map(e)));
        }

        // GET: api/UserSubject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSubject>> GetUserSubject(Guid id)
        {
            var userSubject = _mapper.Map(await _context.UserSubjects.FindAsync(id));

            if (userSubject == null)
            {
                return NotFound();
            }

            return userSubject;
        }

        // PUT: api/UserSubject/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserSubject(Guid id, UserSubject userSubject)
        {
            if (id != userSubject.Id)
            {
                return BadRequest();
            }

            _context.Entry(userSubject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserSubjectExists(id))
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

        // POST: api/UserSubject
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserSubject>> PostUserSubject(UserSubject userSubject)
        {
            _context.UserSubjects.Add(_mapper.Map(userSubject));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserSubject", new { id = userSubject.Id }, userSubject);
        }

        // DELETE: api/UserSubject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSubject(Guid id)
        {
            var userSubject = await _context.UserSubjects.FindAsync(id);
            if (userSubject == null)
            {
                return NotFound();
            }

            _context.UserSubjects.Remove(userSubject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserSubjectExists(Guid id)
        {
            return _context.UserSubjects.Any(e => e.Id == id);
        }
        
        // POST: api/UserSubject
        [HttpPost("user:{userId}/subject:{subjectId}/grade:{grade}")]
        public async Task<ActionResult> SetGrade(Guid userId, Guid subjectId, int grade)
        {
            App.Domain.UserSubject? existing = null;
            try
            {
                existing = await _context.UserSubjects.FirstAsync(
                    u => u.AppUserId == userId && u.SubjectId == subjectId);
            }
            catch (Exception e)
            {
                
            }

            if (existing != null)
            {
                _context.UserSubjects.Remove(existing);
                await _context.SaveChangesAsync();
            }
            
            var userGrade = new UserSubject();
            userGrade.AppUserId = userId;
            userGrade.SubjectId = subjectId;
            userGrade.Grade = grade;
                
            _context.UserSubjects.Add(_mapper.Map(userGrade));
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
