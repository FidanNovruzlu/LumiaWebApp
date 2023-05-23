using LumiaWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LumiaWebApp.DAL
{
    public class LumiaDbContext:IdentityDbContext<AppUser>
    {
        public LumiaDbContext(DbContextOptions<LumiaDbContext> options):base(options)
        {

        }
        public DbSet<Testimonials> Testimonials { get; set; }
        public DbSet<Job > Jobs { get; set; }
    }
}
