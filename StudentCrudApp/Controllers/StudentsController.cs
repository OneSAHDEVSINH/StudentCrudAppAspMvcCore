using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCrudApp.Data;
using StudentCrudApp.Models;

namespace StudentCrudApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;
        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // List all students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.Include(s => s.Addresses).ToListAsync();
            return View(students);
        }

        // GET: Students/Details/5 (Modal content)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .Include(s => s.Addresses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
                return NotFound();

            return PartialView("_DetailsPartial", student);
        }

        // GET: Students/Create (Modal content)

        public IActionResult Create()
        {
            var student = new Student()
            {
                Name = string.Empty,
                Age = 0,
                ProfilePicture = null
            };
            return PartialView("_CreatePartial", student);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student, IFormFile? ProfilePictureFile)
        {
            if (ModelState.IsValid)
            {
                if (ProfilePictureFile != null)
                {
                    using var ms = new MemoryStream();
                    await ProfilePictureFile.CopyToAsync(ms);
                    student.ProfilePicture = ms.ToArray();
                }
                _context.Add(student);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Json(new { isValid = true });

            }
            return PartialView("_CreatePartial", student);
            
        }

        // GET: Students/Edit/5 (Modal content)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();
            return PartialView("_CreatePartial", student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student, IFormFile? ProfilePictureFile)
        {
            if (id != student.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve from DB to update only intended fields.
                    var studentInDb = await _context.Students.FindAsync(id);
                    if (studentInDb == null)
                        return NotFound();

                    studentInDb.Name = student.Name;
                    studentInDb.Age = student.Age;

                    // Update profile picture only if a new file is provided.
                    if (ProfilePictureFile != null)
                    {
                        using var ms = new MemoryStream();
                        await ProfilePictureFile.CopyToAsync(ms);
                        studentInDb.ProfilePicture = ms.ToArray();
                    }
                    _context.Update(studentInDb);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Students.Any(e => e.Id == student.Id))
                        return NotFound();
                    else
                        throw;
                }
                //return RedirectToAction(nameof(Index));
                return Json(new { isValid = true });
            }
            return PartialView("_CreatePartial", student);
            
        }

        // GET: Students/Delete/5 (Modal content)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var student = await _context.Students
                .Include(s => s.Addresses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
                return NotFound();
            return PartialView("_DeletePartial", student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            //return RedirectToAction(nameof(Index));
            return Json(new { isValid = true });
        }
    }
}
