using judge.system.core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

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
        public DbSet<ProblemDetail> ProblemDetails { get; set; }
        public DbSet<ProblemTag> ProblemTags { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new JsonbValueConverter<List<TestCase>>();

            modelBuilder
                .Entity<ProblemDetail>()
                .Property(p => p.TestCases)
                .HasConversion(converter)
                .HasColumnType("jsonb")
                .IsRequired();
            modelBuilder.Entity<ProblemTag>()
                .HasKey(pt => new { pt.ProblemId, pt.TagId });

            modelBuilder.Entity<ProblemTag>()
                .HasOne(pt => pt.Problem)
                .WithMany(p => p.ProblemTags)
                .HasForeignKey(pt => pt.ProblemId);

            modelBuilder.Entity<ProblemTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProblemTags)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<Problem>()
                    .HasMany(p => p.Submissions)
                    .WithOne(s => s.Problem)
                    .HasForeignKey(s => s.ProblemId);
        }

        public class JsonbValueConverter<T> : ValueConverter<T, string>
        {
            public JsonbValueConverter()
                : base(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<T>(v))
            {
            }
        }
    }
}
