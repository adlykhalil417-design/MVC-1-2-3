
using GymManagement.DAL.DATA.DataSeeding;
using GYMMANGEMANETPROGECTMV01;
using GYMMANGEMANETPROGECTMVC01.DAL.DATA.DataSeeding;
using GYMMANGEMANETPROGECTMVC01.DAL.DATA.DbContexts;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GYMMANGEMANETPROGECTMVC01.PL
{
    public  static  class ProgramExtensions
    {

        public static async Task MigrateAndSeedDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            //dashboard
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();





            var pendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Applying {pendingMigrations.Count()} Pending Migrations");
                await dbcontext.Database.MigrateAsync();
            }


            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbcontext, seedFolderPath, logger);

            //dashboard
            await IdentityDataSeeding.SeedIdentityDataAsync(roleManager, userManager, logger);


        }
    }
}
