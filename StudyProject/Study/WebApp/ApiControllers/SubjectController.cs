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
    public class SubjectController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Subject, App.Domain.Subject> _mapper;

        public SubjectController(AppDbContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Subject, App.Domain.Subject>(autoMapper);
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            return Ok((await _context.Subjects.ToListAsync()).Select(e => _mapper.Map(e)));
        }

        // GET: api/Subject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(Guid id)
        {
            var subject = _mapper.Map(await _context.Subjects.FindAsync(id));

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }
        // GET: api/Subject/5
        [HttpGet("semester:{semesterId}")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsInSemester(Guid semesterId)
        {
            var semester = await _context.Semesters.FindAsync(semesterId);

            if (semester == null)
            {
                return NotFound();
            }

            var subjects = (await _context.Subjects.Where(s => s.SemesterId == semester.Id)
                .ToListAsync()).Select(e => _mapper.Map(e));
            return Ok(subjects);
        }
        
        // GET: api/Subject/5
        [HttpGet("average/{subjectId}")]
        public async Task<ActionResult<float>> GetAverageGrade(Guid subjectId)
        {
            var subjectResults = await _context.UserSubjects.Where(e => e.SubjectId == subjectId).ToListAsync();

            int count = 0;
            float sum = 0;

            foreach (var result in subjectResults)
            {
                sum += result.Grade;
                count++;
            }

            if (count == 0)
            {
                return Ok(0);
            }
            else
            {
                return Ok(sum/count);
            }
        }
        
        // GET: api/Subject/5
        [HttpGet("subject:{subjectId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetSubjectUsers(Guid subjectId)
        {

            var users = await _context.Users.Where(u =>
                _context.UserSubjects.Any(us => us.AppUserId == u.Id && us.SubjectId == subjectId)).ToListAsync();

            var dtoUsers = new List<User>();
            
            for (int i = 0; i < users.Count; i++)
            {
                var tempUser = new User();
                tempUser.Id = users[i].Id;
                tempUser.Email = users[i].Email;
                dtoUsers.Add(tempUser);
            }

            return Ok(dtoUsers);
        }
        
        // GET: api/Subject/5
        [HttpGet("subject:{subjectId}/user:{userId}")]
        public async Task<ActionResult<int>> GetGrade(Guid subjectId, Guid userId)
        {

            try
            {
                var userGrade =
                    await _context.UserSubjects.FirstAsync(u => u.AppUserId == userId && u.SubjectId == subjectId);

                if (userGrade != null && userGrade.Grade > 0)
                {
                    return Ok(userGrade.Grade);
                }
                return Ok(0);
            }
            catch (Exception e)
            {

                return Ok(0);
            }
        }

        // PUT: api/Subject/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(Guid id, Subject subject)
        {
            if (id != subject.Id)
            {
                return BadRequest();
            }

            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST: api/Subject
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubject", new { id = subject.Id }, subject);
        }

        // DELETE: api/Subject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool SubjectExists(Guid id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
