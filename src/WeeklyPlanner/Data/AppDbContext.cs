using Microsoft.EntityFrameworkCore;

namespace WeeklyPlanner.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=LocalDatabase.db;");
            }
        }


        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<ScheduledTask> ScheduledTasks { get; set; }

        public virtual DbSet<ScheduledTaskTransfer> ScheduledTaskTransfers { get; set; }
    }
}