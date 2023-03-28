using Microsoft.EntityFrameworkCore;
using Contacts.Models;

//using database
namespace Contacts.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        { 
        }

        public DbSet<Contact> Contacts { get; set; }

        
    }
}
