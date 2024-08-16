using Announcement_Test_Task.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Announcement_Test_Task.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; }
    }
}
