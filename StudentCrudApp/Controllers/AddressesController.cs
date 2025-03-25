using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCrudApp.Data;
using StudentCrudApp.Models;

namespace StudentCrudApp.Controllers
{
    public class AddressesController : Controller
    {
        private readonly AppDbContext _context;
        public AddressesController(AppDbContext context)
        {
            _context = context;
        }

        // List addresses for a particular student
        public async Task<IActionResult> Index(int studentId)
        {
            ViewBag.StudentId = studentId;
            var addresses = await _context.Addresses
                .Where(a => a.StudentId == studentId)
                .ToListAsync();
            return View(addresses);
        }

        // GET: Addresses/Details/5 (Modal content)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var address = await _context.Addresses.Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.AddressID == id);
            if (address == null)
                return NotFound();

            return PartialView("_DetailsPartial", address);
        }

        // GET: Addresses/Create?studentId=5 (Modal content)
        public IActionResult Create(int studentId)
        {
            var address = new Address
            {
                StudentId = studentId,
                AddressLine = string.Empty,
                City = string.Empty,
                State = string.Empty,
                PinCode = string.Empty
            };
            return PartialView("_CreatePartial", address);
        }

        // POST: Addresses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                //return RedirectToAction("Index", new { studentId = address.StudentId });
                return Json(new { isValid = true });
            }
            return PartialView("_CreatePartial", address);
        }

        // GET: Addresses/Edit/5 (Modal content)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
                return NotFound();
            return PartialView("_CreatePartial", address);
        }

        // POST: Addresses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Address address)
        {
            if (id != address.AddressID)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Addresses.Any(a => a.AddressID == address.AddressID))
                        return NotFound();
                    else
                        throw;
                }
                //return RedirectToAction("Index", new { studentId = address.StudentId });
                return Json(new { isValid = true });
            }
            return PartialView("_CreatePartial", address);
        }

        // GET: Addresses/Delete/5 (Modal content)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var address = await _context.Addresses.Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.AddressID == id);
            if (address == null)
                return NotFound();
            return PartialView("_DeletePartial", address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                int studentId = address.StudentId;
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                //return RedirectToAction("Index", new { studentId });
                return Json(new { isValid = true });
            }
            return NotFound();
        }
    }
}
