using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace SvcService.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ServiceDbContext>
    {
        public ServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServiceDbContext>();
            var connectionString = "server=localhost; port=3306; uid=mcsdbg; pwd=MyWceQc-cFgPynao; database=svcservice";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new(optionsBuilder.Options);
        }
    }

}
