using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConferenceSystem.Models;
using ConferenceSystem.data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ConferenceSystem.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ConfeSystemData _context;

        public RegistrationController(ConfeSystemData context)
        {
            _context = context;
        }
        
        [Authorize(Roles = "Attendee,Admin")]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var events = _context.EventSeminar
                .Include(e => e.CreatedByUser)
                .AsQueryable(); // Convert to IQueryable for further filtering

            if (!string.IsNullOrEmpty(searchString))
            {
                events = events.Where(e => e.EventName.Contains(searchString));
            }
            else
            {
                // Return an empty result when the search string is empty
                events = events.Where(e => false);
            }

            return View(await events.ToListAsync());
        }


        // GET: Registration
        public async Task<IActionResult> Index()
        {
            var confeSystemData = _context.Registration.Include(r => r.Event);
            return View(await confeSystemData.ToListAsync());
        }

        // GET: Registration/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Registration == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration
                .Include(r => r.Event)
                .FirstOrDefaultAsync(m => m.RegistrationId == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }
        [Authorize(Roles = "Attendee,Admin")]
        // GET: Registration/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.EventSeminar, "EventId", "EventName");
            return View();
        }

        // POST: Registration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationId,FullName,Email,EventId")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.EventSeminar, "EventId", "EventName", registration.EventId);
            return View(registration);
        }
        [Authorize(Roles = "Attendee,Admin")]
        // GET: Registration/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Registration == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.EventSeminar, "EventId", "EventLocation", registration.EventId);
            return View(registration);
        }

        // POST: Registration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegistrationId,FullName,Email,EventId")] Registration registration)
        {
            if (id != registration.RegistrationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.RegistrationId))
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
            ViewData["EventId"] = new SelectList(_context.EventSeminar, "EventId", "EventLocation", registration.EventId);
            return View(registration);
        }
        [Authorize(Roles = "Attendee,Admin")]
        // GET: Registration/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Registration == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration
                .Include(r => r.Event)
                .FirstOrDefaultAsync(m => m.RegistrationId == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Registration == null)
            {
                return Problem("Entity set 'ConfeSystemData.Registration'  is null.");
            }
            var registration = await _context.Registration.FindAsync(id);
            if (registration != null)
            {
                _context.Registration.Remove(registration);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
          return (_context.Registration?.Any(e => e.RegistrationId == id)).GetValueOrDefault();
        }
    }
}
