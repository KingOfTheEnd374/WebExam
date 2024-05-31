using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Areas_Admin_Controllers
{
    [Area("Admin")]
    public class SubjectController : Controller
    {
        private readonly AppDbContext _context;

        public SubjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Subject
        public async Task<IActionResult> Index()
        {
            //ViewData["Users"] = new SelectList(await _context.Users.ToListAsync(), "Id", "Username");
            var appDbContext = _context.Subjects.Include(s => s.Semester);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Subject/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Semester)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            ViewData["Users"] = new SelectList(await _context.Users.Where(u => !_context.UserSubjects.Any(us => us.SubjectId == id && us.AppUserId == u.Id)).ToListAsync(), "Id", "Username");
            ViewData["AlreadyUsers"] = new SelectList(await _context.Users.Where(u => _context.UserSubjects.Any(us => us.AppUserId == u.Id && us.SubjectId == id)).ToListAsync(), "Id", "Username");

            return View(subject);
        }

        // GET: Subject/Create
        public IActionResult Create()
        {
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
            return View();
        }

        // POST: Subject/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SemesterId,Id")] Subject subject)
        {
                subject.Id = Guid.NewGuid();
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Subject/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name", subject.SemesterId);
            return View(subject);
        }

        // POST: Subject/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,SemesterId,Id")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }
            
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
        }

        // GET: Subject/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Semester)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(Guid id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(Guid subjectId, Guid userId)
        {
            UserSubject userConnection = new UserSubject();
            userConnection.AppUserId = userId;
            userConnection.SubjectId = subjectId;
            await _context.UserSubjects.AddAsync(userConnection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUser(Guid subjectId, Guid userId)
        {
            var a = _context.UserSubjects.First(us => us.AppUserId == userId && us.SubjectId == subjectId);
            _context.Remove(a);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
