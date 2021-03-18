using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Valgapplikasjon.Areas.Identity;

namespace Valgapplikasjon.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ValgapplikasjonUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seeder Roller
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Kontrollor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Bruker.ToString()));
        }
        public static async Task SeedSuperAdminAsync(UserManager<ValgapplikasjonUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seeder Default Bruker
            var defaultUser = new ValgapplikasjonUser
            {
                UserName = "valgapplikasjon@gmail.com",
                Email = "valgapplikasjon@gmail.com",
                Fornavn = "Valgapp",
                Etternavn = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "tEst666/");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Bruker.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Kontrollor.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                }

            }
        }
    }
}
