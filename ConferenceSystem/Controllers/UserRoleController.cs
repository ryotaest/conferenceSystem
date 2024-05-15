using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConferenceSystem.Models;
using ConferenceSystem.data;

namespace ConferenceSystem.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly ConfeSystemData _context;

        public UserRoleController(ConfeSystemData context)
        {
            _context = context;
        }

        // GET: UserRole
        public async Task<IActionResult> Index()
        {
            var confeSystemData = _context.UserRole.Include(u => u.Role).Include(u => u.User);
            return View(await confeSystemData.ToListAsync());
        }

        // GET: UserRole/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRole
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // GET: UserRole/Create
        public IActionResult Create()
        {
            ViewData["RoleID"] = new SelectList(_context.Role, "Id", "RoleName");
            ViewData["UserID"] = new SelectList(_context.User, "UserId", "FullName");
            return View();
        }

        // POST: UserRole/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserID,RoleID")] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleID"] = new SelectList(_context.Role, "Id", "RoleName", userRole.RoleID);
            ViewData["UserID"] = new SelectList(_context.User, "UserId", "FirstName", userRole.UserID);
            return View(userRole);
        }

        // GET: UserRole/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRole.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }
            ViewData["RoleID"] = new SelectList(_context.Role, "Id", "Id", userRole.RoleID);
            ViewData["UserID"] = new SelectList(_context.User, "UserId", "ConfirmPassword", userRole.UserID);
            return View(userRole);
        }

        // POST: UserRole/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,RoleID")] UserRole userRole)
        {
            if (id != userRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRoleExists(userRole.Id))
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
            ViewData["RoleID"] = new SelectList(_context.Role, "Id", "Id", userRole.RoleID);
            ViewData["UserID"] = new SelectList(_context.User, "UserId", "ConfirmPassword", userRole.UserID);
            return View(userRole);
        }

        // GET: UserRole/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRole
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // POST: UserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRole == null)
            {
                return Problem("Entity set 'ConfeSystemData.UserRole'  is null.");
            }
            var userRole = await _context.UserRole.FindAsync(id);
            if (userRole != null)
            {
                _context.UserRole.Remove(userRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRoleExists(int id)
        {
          return (_context.UserRole?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
