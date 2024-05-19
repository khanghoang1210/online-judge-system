using judge.system.core.Models;
using Microsoft.EntityFrameworkCore;

namespace judge.system.core.Database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //    .Entity<Problem>()
        //    .Property(p => p.TestCases)
        //    .HasColumnType("jsonb")
        //    .IsRequired();

        //}
    }
}
