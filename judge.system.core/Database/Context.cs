using judge.system.core.Models;
using Microsoft.EntityFrameworkCore;

namespace judge.system.core.Database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
    }
}
