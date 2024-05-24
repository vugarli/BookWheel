using BookWheel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure
{
    public static class ApplicationIdentityDbContextSeed
    {
        public static async Task SeedAsync(ApplicationIdentityDbContext identityDbContext, RoleManager<IdentityRole<Guid>> roleManager)
        {

            if (identityDbContext.Database.IsSqlServer())
            {
                identityDbContext.Database.Migrate();
            }

            if(await roleManager.FindByNameAsync("Owner") is null) 
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Owner"));
                await roleManager.CreateAsync(new IdentityRole<Guid>("Customer"));
            }


            //var defaultUser = new ApplicationUser { UserName = AppConstants.REG_USERNAME, Email = AppConstants.REG_USERNAME };
            //await userManager.CreateAsync(defaultUser, AppConstants.REG_PASSWORD);



            //var adminUser = new ApplicationUser { UserName = AppConstants.SUPERADMIN_USERNAME, Email = AppConstants.SUPERADMIN_USERNAME };

            //await userManager.CreateAsync(adminUser, AppConstants.SUPERADMIN_PASSWORD);

            //adminUser = await userManager.FindByNameAsync(AppConstants.SUPERADMIN_USERNAME);

            //if (adminUser != null)
            //{
            //    await userManager.AddToRoleAsync(adminUser, AppConstants.Roles.SUPERADMINROLE);
            //}
        }
    }
}
