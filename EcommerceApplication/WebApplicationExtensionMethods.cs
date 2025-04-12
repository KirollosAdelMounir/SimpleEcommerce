using EcommerceData.DBContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApplication
{
    public static class WebApplicationExtensionMethods
    {
        public static void MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<EcommerceContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
