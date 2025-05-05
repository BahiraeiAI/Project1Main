using System;
using Microsoft.AspNetCore.Identity;

namespace Project1.Data
{
	public class SeedData
	{
		public static async Task RoleSeed(IApplicationBuilder _Application)
		{
			using (var Scope = _Application.ApplicationServices.CreateScope())
			{
				var _RoleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await _RoleManager.RoleExistsAsync(UserRoles.Admin))
                     await _RoleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
				if (!await _RoleManager.RoleExistsAsync(UserRoles.User))
					await _RoleManager.CreateAsync(new IdentityRole(UserRoles.User));
			}
        }

	}
}

