using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GYMMANGEMANETPROGECTMVC01.DAL.DATA.DbContexts
{
    public class GymDbContext:DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext>options) :base(options){
        
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-9I8CESJ;Database=GymManagementMinic01;Trusted_Connection=true;TrustServerCertificate=true");


        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
        }
        public DbSet<Plan>Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
