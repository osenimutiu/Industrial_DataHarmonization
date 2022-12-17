using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCBHarmonization.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace SCBHarmonization.IdentityExtentions
{
    public class AppUserManager : UserManager<ApplicationUser>
    {
        private const int PASSWORD_HISTORY_LIMIT = 5;

        public AppUserManager()
           : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
            PasswordValidator = new CustomizePasswordValidation(12);
        }

        //public override async Task<IdentityResult> ChangePasswordAsync(string UserID, string CurrentPassword, string NewPassword)
        //{
        //    if (await IsPreviousPassword(UserID, NewPassword))
        //    {
        //        return await Task.FromResult(IdentityResult.Failed("You Cannot Reuse Previous Password"));
        //    }

        //    var Result = await base.ChangePasswordAsync(UserID, CurrentPassword, NewPassword);

        //    if (Result.Succeeded)
        //    {
        //        var appStore = Store as AppUserStore;
        //        await appStore.AddToUsedPasswordAsync(await FindByIdAsync(UserID), PasswordHasher.HashPassword(NewPassword));
        //    }

        //    return Result;
        //}

        //public override async Task<IdentityResult> ResetPasswordAsync(string UserID, string UsedToken, string NewPassword)
        //{
        //    if (await IsPreviousPassword(UserID, NewPassword))
        //    {
        //        return await Task.FromResult(IdentityResult.Failed("You Cannot Reuse Previous Password"));
        //    }

        //    var Result = await base.ResetPasswordAsync(UserID, UsedToken, NewPassword);

        //    if (Result.Succeeded)
        //    {
        //        var appStore = Store as AppUserStore;
        //        await appStore.AddToUsedPasswordAsync(await FindByIdAsync(UserID), PasswordHasher.HashPassword(NewPassword));
        //    }

        //    return Result;
        //}

        private async Task<bool> IsPreviousPassword(string UserID, string NewPassword)
        {
            //var User = await FindByIdAsync(UserID);
            //if (User.UserUsedPassword.OrderByDescending(x => x.CreatedDate).
            //    Select(x => x.HashPassword).Take(PASSWORD_HISTORY_LIMIT).Where(x => PasswordHasher.VerifyHashedPassword(x, NewPassword) != PasswordVerificationResult.Failed).Any())
            //{
            //    return true;
            //}


                //if (User.UserUsedPassword.OrderByDescending(up => up.CreatedDate).Select(up => up.HashPassword).Take(UsedPasswordLimit).Where(up => PasswordHasher.VerifyHashedPassword(up, NewPassword) != PasswordVerificationResult.Failed).Any())
                //{
                //    return true;
                //}

                return false;
        }
    }
}