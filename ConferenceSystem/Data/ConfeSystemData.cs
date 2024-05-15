using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceSystem.Models;

namespace ConferenceSystem.data
{
    public class ConfeSystemData : DbContext
    {

        public ConfeSystemData (DbContextOptions<ConfeSystemData> options)
            : base(options)
        {
        }

        public DbSet<ConferenceSystem.Models.User> User { get; set; } = default!;

        public DbSet<ConferenceSystem.Models.Role> Role { get; set; } = default!;

        public DbSet<ConferenceSystem.Models.UserRole> UserRole { get; set; } = default!;

        public DbSet<ConferenceSystem.Models.EventSeminar> EventSeminar { get; set; } = default!;

        public DbSet<ConferenceSystem.Models.Registration> Registration { get; set; } = default!;
      
    }
}
