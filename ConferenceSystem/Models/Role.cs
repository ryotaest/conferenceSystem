using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ConferenceSystem.Models
{
	public class Role
	{
        public int Id { get; set; }

        public string ?RoleName { get; set; }

        public virtual ICollection<UserRole> ?UserRole { get; set; }

    }
}

