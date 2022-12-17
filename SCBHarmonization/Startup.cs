using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SCBHarmonization.Models;

[assembly: OwinStartupAttribute(typeof(SCBHarmonization.Startup))]
namespace SCBHarmonization
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
			createRolesandUsers();
		}

		private void createRolesandUsers()
		{
			ApplicationDbContext context = new ApplicationDbContext();

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
			if (!roleManager.RoleExists("Opal-Checker"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Opal-Checker";
				roleManager.Create(role);	
				var user = new ApplicationUser();
				user.UserName = "1449388";
				user.StaffName = "Ogbonna Ifeanyi";
				user.Email = "Ifeanyi.Ogbonna@sc.com";
				string userPWD = "Password10$";

				var chkUser = UserManager.Create(user, userPWD);
				if (chkUser.Succeeded)
				{
					var result1 = UserManager.AddToRole(user.Id, "Opal-Checker");

				}
			}
			if (!roleManager.RoleExists("eBBS-Checker"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "eBBS-Checker";
				roleManager.Create(role);
			}
			if (!roleManager.RoleExists("System Admin"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "System Admin";
				roleManager.Create(role);
			}
			if (!roleManager.RoleExists("Opal-Maker"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Opal-Maker";
				roleManager.Create(role);
			}

			if (!roleManager.RoleExists("eBBS-Maker"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "eBBS-Maker";
				roleManager.Create(role);
			}
		}

	}
}
