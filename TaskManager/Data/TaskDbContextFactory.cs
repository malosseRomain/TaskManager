using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TaskManager.Data;

namespace TaskManager
{
    public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
    {
        public TaskDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
            optionsBuilder.UseMySql(
                "server=localhost;database=taskmaster;user=root;password=root",
                new MySqlServerVersion(new Version(8, 0, 30)));

            return new TaskDbContext(optionsBuilder.Options);
        }
    }
}
