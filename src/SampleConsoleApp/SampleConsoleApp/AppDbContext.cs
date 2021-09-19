using Microsoft.EntityFrameworkCore;
using SampleConsoleApp.Models;

namespace SampleConsoleApp
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        protected AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlRaw(sql, parameters);
        }
    }
}
