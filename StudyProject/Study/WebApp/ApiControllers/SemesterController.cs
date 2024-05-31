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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SemesterController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Semester, App.Domain.Semester> _mapper;
        //private readonly PublicDTOBllMapper<App.DTO.v1_0.Subject, App.Domain.Subject> _subjectMapper;

        public SemesterController(AppDbContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Semester, App.Domain.Semester>(autoMapper);
            //_subjectMapper = new PublicDTOBllMapper<App.DTO.v1_0.Subject, App.Domain.Subject>(autoMapper);
        }

        // GET: api/Semester
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semester>>> GetSemesters()
        {
            return Ok((await _context.Semesters.ToListAsync()).Select(e => _mapper.Map(e)));
        }

        // GET: api/Semester/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsInSemester(Guid id)
        {
            var semester = _mapper.Map(await _context.Semesters.FindAsync(id));

            if (semester == null)
            {
                return NotFound();
            }

            var subjects = (await _context.Subjects.Where(s => s.SemesterId == semester.Id)
                .ToListAsync()).Select(e => _subjectMapper.Map(e));
            return Ok(subjects);
        }*/

        // PUT: api/Semester/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutSemester(Guid id, Semester semester)
        {
            if (id != semester.Id)
            {
                return BadRequest();
            }

            _context.Entry(semester).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SemesterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Semester
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        public async Task<ActionResult<Semester>> PostSemester(Semester semester)
        {
            _context.Semesters.Add(semester);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSemester", new { id = semester.Id }, semester);
        }*/

        // DELETE: api/Semester/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(Guid id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null)
            {
                return NotFound();
            }

            _context.Semesters.Remove(semester);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool SemesterExists(Guid id)
        {
            return _context.Semesters.Any(e => e.Id == id);
        }
    }
}
