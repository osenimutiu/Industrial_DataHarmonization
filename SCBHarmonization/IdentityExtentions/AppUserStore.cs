using Microsoft.AspNet.Identity.EntityFramework;
using SCBHarmonization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;


namespace SCBHarmonization.IdentityExtentions
{
    public class AppUserStore : UserStore<ApplicationUser>
    {
        //public AppUserStore(DbContext MyDbContext)
        //    : base(MyDbContext)
        //{
        //}

        //public override async Task CreateAsync(ApplicationUser appuser)
        //{
        //    await base.CreateAsync(appuser);
        //    await AddToUsedPasswordAsync(appuser, appuser.PasswordHash);
        //    //await AddToPreviousPasswordsAsync(appuser, appuser.PasswordHash);

        //}

        //public Task AddToUsedPasswordAsync(ApplicationUser appuser, string userpassword)
        //{
        //    appuser.UserUsedPassword.Add(new UsedPassword() { UserID = appuser.Id, HashPassword = userpassword });
        //    return UpdateAsync(appuser);
        //}
        //public Task AddToPreviousPasswordsAsync(ApplicationUser user, string password)
        //{
        //    user.UserUsedPassword.Add(new UsedPassword() { UserID = user.Id, HashPassword = password });
        //    return UpdateAsync(user);
        //}
    }
}