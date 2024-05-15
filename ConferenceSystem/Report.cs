using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceSystem.data;
using ConferenceSystem.Models;

namespace ConferenceSystem.Reports
{
    public class MonthlyEventReport
    {
        private readonly ConfeSystemData _dbContext;

        public MonthlyEventReport(ConfeSystemData dbContext)
        {
            _dbContext = dbContext;
        }

        public List<IGrouping<DateTime, EventSeminar>> GetMonthlyEvents(int year, int month)
        {
            // Retrieve events for the specified month
            var events = _dbContext.EventSeminar
                .Where(e => e.EventTime.Year == year && e.EventTime.Month == month)
                .ToList();

            // Group events by event date
            var groupedEvents = events.GroupBy(e => e.EventTime.Date).ToList();

            return groupedEvents;
        }
    }
}
