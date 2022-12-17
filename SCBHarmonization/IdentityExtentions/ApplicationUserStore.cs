using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SCBHarmonization.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SCBHarmonization.IdentityExtentions
{
    public class ApplicationUserStore : UserStore<ApplicationUser>

    {
        //public ApplicationUserStore(DbContext context)
        //    : base(context)
        //{
        //}
        //public override async Task CreateAsync(ApplicationUser user)
        //{
        //    await base.CreateAsync(user);
        //    await AddToPreviousPasswordsAsync(user, user.PasswordHash);
        //}
        //public Task AddToPreviousPasswordsAsync(ApplicationUser user, string password)
        //{
        //    user.UserUsedPassword.Add(new UsedPassword() { UserID = user.Id, HashPassword = password });
        //    return UpdateAsync(user);
        //}
    }
}