﻿using System;
namespace ConferenceSystem.Models
{
	public class UserRole
	{
        public int Id { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public virtual User ?User { get; set; }
        public virtual Role ?Role { get; set; }

    }
}

