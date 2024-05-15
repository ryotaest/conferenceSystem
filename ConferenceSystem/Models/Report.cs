using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConferenceSystem.Models
{
	public class Report
	{
        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }
        public List<SelectListItem> ?AvailableYears { get; set; }
        public List<SelectListItem> ?AvailableMonths { get; set; }
        public List<EventSeminar> ?MonthlyEvents { get; set; }
    }
}

