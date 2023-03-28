using Microsoft.EntityFrameworkCore;
using Contacts.Models;

//using database
namespace Contacts.Data
{
    public class LoginDbContext : DbContext
    {
        internal IEnumerable<object> login;

        public LoginDbContext(DbContextOptions<LoginDbContext> options) :base(options)
        { 
        }

        public DbSet<LoginData> Login { get; set; }

        
    }
}
