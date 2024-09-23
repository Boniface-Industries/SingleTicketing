using Microsoft.EntityFrameworkCore;

namespace SingleTicketing.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Enforcer> Enforcers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Violation> Violations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleName)       
                .HasPrincipalKey(r => r.RoleName);     

            modelBuilder.Entity<User>()
                .HasOne(u => u.Status)
                .WithMany()
                .HasForeignKey(u => u.StatusName)       
                .HasPrincipalKey(s => s.StatusName);  
        }


    }


}
