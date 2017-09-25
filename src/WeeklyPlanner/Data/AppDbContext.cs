using Microsoft.EntityFrameworkCore;

namespace WeeklyPlanner.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS; Initial Catalog=WeeklyPlanner; Integrated Security=true");
            }
        }


        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<ScheduledTask> ScheduledTasks { get; set; }

        public virtual DbSet<ScheduledTaskTag> ScheduledTaskTags { get; set; }

        public virtual DbSet<ScheduledTaskTransfer> ScheduledTaskTransfers { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }
    }
}