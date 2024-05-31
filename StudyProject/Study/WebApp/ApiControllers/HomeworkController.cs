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
    public class HomeworkController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Homework, App.Domain.Homework> _mapper;

        public HomeworkController(AppDbContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Homework, App.Domain.Homework>(autoMapper);
        }

        // GET: api/Homework
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Homework>>> GetHomeworks()
        {
            return Ok((await _context.Homeworks.ToListAsync()).Select(e => _mapper.Map(e)));
        }

        // GET: api/Homework/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Homework>> GetHomework(Guid id)
        {
            var homework = _mapper.Map(await _context.Homeworks.FindAsync(id));

            if (homework == null)
            {
                return NotFound();
            }

            return homework;
        }

        // PUT: api/Homework/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHomework(Guid id, Homework homework)
        {
            if (id != homework.Id)
            {
                return BadRequest();
            }

            _context.Entry(homework).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkExists(id))
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

        // POST: api/Homework
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Homework>> PostHomework(Homework homework)
        {
            _context.Homeworks.Add(_mapper.Map(homework));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHomework", new { id = homework.Id }, homework);
        }

        // DELETE: api/Homework/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHomework(Guid id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }

            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HomeworkExists(Guid id)
        {
            return _context.Homeworks.Any(e => e.Id == id);
        }
        
        // GET: api/Subject/5
        [HttpGet("subject:{subjectId}")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsInSemester(Guid subjectId)
        {
            var subject = await _context.Subjects.FindAsync(subjectId);

            if (subject == null)
            {
                return NotFound();
            }

            var homeworks = (await _context.Homeworks.Where(h => h.SubjectId == subject.Id)
                .ToListAsync()).Select(e => _mapper.Map(e));
            return Ok(homeworks);
        }
        
        // GET: api/Subject/5
        [HttpGet("average/{homeworkId}")]
        public async Task<ActionResult<float>> GetAverageGrade(Guid homeworkId)
        {
            var hwResult = await _context.UserHomeworks.Where(e => e.HomeworkId == homeworkId).ToListAsync();

            int count = 0;
            float sum = 0;

            foreach (var result in hwResult)
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
        [HttpGet("hw:{hwId}/user:{userId}")]
        public async Task<ActionResult<int>> GetGrade(Guid hwId, Guid userId)
        {
            try
            {
                var userGrade =
                    await _context.UserHomeworks.FirstAsync(u => u.AppUserId == userId && u.HomeworkId == hwId);

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
    }
}
