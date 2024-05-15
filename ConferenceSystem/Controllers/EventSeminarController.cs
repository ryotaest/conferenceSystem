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
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Diagnostics;



namespace ConferenceSystem.Controllers
{
    public class EventSeminarController : Controller
    {
        private readonly ConfeSystemData _context;

        public EventSeminarController(ConfeSystemData context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]

        public IActionResult Report()
        {
           

            var reportModel = new Report();

            
            reportModel.AvailableYears = GetAvailableYears().ToList();
            reportModel.AvailableMonths = GetAvailableMonths().ToList();

            // Set the SelectedYear value as a collection of select list items

            return View(reportModel);
        }



        [HttpPost]
        public IActionResult GenerateReport(Report report)
        {
            // Get the selected year and month from the report model
            int selectedYear = report.SelectedYear;
            int selectedMonth = report.SelectedMonth;

            // Retrieve the events for the selected year and month from the database
            List<EventSeminar> monthlyEvents = _context.EventSeminar
                .Where(e => e.EventTime.Year == selectedYear && e.EventTime.Month == selectedMonth)
                .ToList();

            // Include the related CreatedByUser entity for each event
            monthlyEvents.ForEach(e => _context.Entry(e).Reference(x => x.CreatedByUser).Load());

            
            report.MonthlyEvents = monthlyEvents;

            report.AvailableYears = GetAvailableYears().ToList();
            report.AvailableMonths = GetAvailableMonths().ToList();

            // Set the selected year and month in the model
            report.SelectedYear = selectedYear;
            report.SelectedMonth = selectedMonth;

            return View("Report", report);
        }

        private List<SelectListItem> GetAvailableYears()
        {
            var years = new List<SelectListItem>();

            // Populate the years as per your requirements
            for (int i = DateTime.Now.Year; i >= 2010; i--)
            {
                years.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }

            return years;
        }

        private List<SelectListItem> GetAvailableMonths()
        {
            var months = new List<SelectListItem>();

            // Populate the months
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new SelectListItem { Value = i.ToString(), Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) });
            }

            return months;
        }





        // GET: EventSeminar
        public async Task<IActionResult> Index()
        {
            var confeSystemData = _context.EventSeminar.Include(e => e.CreatedByUser);
            return View(await confeSystemData.ToListAsync());
        }

        // GET: EventSeminar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EventSeminar == null)
            {
                return NotFound();
            }

            var eventSeminar = await _context.EventSeminar
                .Include(e => e.CreatedByUser)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (eventSeminar == null)
            {
                return NotFound();
            }

            return View(eventSeminar);
        }
        [Authorize(Roles = "Admin")]
        // GET: EventSeminar/Create
        public IActionResult Create()
        {
            ViewData["CreatedByUserUserId"] = new SelectList(_context.User, "UserId", "FullName");
            return View();
        }

        // POST: EventSeminar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,RegistrationFees,MaxAttendees,EventTime,EventLocation,CreatedByUserUserId")] EventSeminar eventSeminar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventSeminar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedByUserUserId"] = new SelectList(_context.User, "UserId", "FullName", eventSeminar.CreatedByUserUserId);
            return View(eventSeminar);
        }
        [Authorize(Roles = "Admin")]
        // GET: EventSeminar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EventSeminar == null)
            {
                return NotFound();
            }

            var eventSeminar = await _context.EventSeminar.FindAsync(id);
            if (eventSeminar == null)
            {
                return NotFound();
            }
            ViewData["CreatedByUserUserId"] = new SelectList(_context.User, "UserId", "FullName", eventSeminar.CreatedByUserUserId);
            return View(eventSeminar);
        }

        // POST: EventSeminar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,RegistrationFees,MaxAttendees,EventTime,EventLocation,CreatedByUserUserId")] EventSeminar eventSeminar)
        {
            if (id != eventSeminar.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventSeminar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventSeminarExists(eventSeminar.EventId))
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
            ViewData["CreatedByUserUserId"] = new SelectList(_context.User, "UserId", "FullName", eventSeminar.CreatedByUserUserId);
            return View(eventSeminar);
        }
        [Authorize(Roles = "Admin")]
        // GET: EventSeminar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EventSeminar == null)
            {
                return NotFound();
            }

            var eventSeminar = await _context.EventSeminar
                .Include(e => e.CreatedByUser)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (eventSeminar == null)
            {
                return NotFound();
            }

            return View(eventSeminar);
        }

        // POST: EventSeminar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EventSeminar == null)
            {
                return Problem("Entity set 'ConfeSystemData.EventSeminar'  is null.");
            }
            var eventSeminar = await _context.EventSeminar.FindAsync(id);
            if (eventSeminar != null)
            {
                _context.EventSeminar.Remove(eventSeminar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventSeminarExists(int id)
        {
          return (_context.EventSeminar?.Any(e => e.EventId == id)).GetValueOrDefault();
        }
    }
}
