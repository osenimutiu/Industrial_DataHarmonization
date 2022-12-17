using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.SqlServer.Management.Smo;
using SCBHarmonization.CustomFilter;
using SCBHarmonization.Helper;
using SCBHarmonization.Models;
using SCBHarmonization.ViewModels;
using static SCBHarmonization.LicenseValidator;

namespace SCBHarmonization.Controllers
{
   
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        SCBDBEntities objDbEntities = new SCBDBEntities();
        ApplicationDbContext context;
        //public AccountController() : this(new AppUserManager())
        //{
        //}
        public AccountController()
        {
            context = new ApplicationDbContext();
        }

        //public AccountController(AppUserManager userManager)
        //{
        //    UserManager = userManager;
        //}

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        //public AppUserManager UserManager
        //{
        //    get;
        //    private set;
        //}

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        //[AllowAnonymous]
        //public ActionResult Login(string returnUrl)
        //{
        //    LicenseValidator licenseValidator = new LicenseValidator();
        //    licenseValidator.ExamineLicense();
        //    ViewBag.ReturnUrl = returnUrl;

        //    if (licenseValidator.ExamineLicense().Status != LicenseStatus.ValidLicense)
        //    {
        //        if (licenseValidator.ExamineLicense().ExpireDate < licenseValidator.sendMailDate)
        //        {
        //            licenseValidator.LicenseExpire();
        //        }
        //        return View();
        //    }
        //    else
        //    {

        //        return RedirectToAction("LicenseError", "Home");

        //    }

        //}


        //public ActionResult Audit()
        //{
        //    string UserId = User.Identity.GetUserId();
        //    ApplicationUser user = UserManager.FindById(UserId);
        //    string Url = Request.Url.AbsoluteUri;
        //    string UserName = user.UserName;
        //    string Detail = $"This action is performed by user with PSID '{UserName}'";
        //    string IPAddress = SendEmail.GetLocalIpAddress();
        //    string DeviceName = SendEmail.GetDeviceName();
        //    string OSName = SendEmail.FriendlyName();
        //    return View();
        //}
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {  
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //user.HasLoggedIn = true;
            //var resulta = await UserManager.UpdateAsync(user);
            var user1 = await UserManager.FindByNameAsync(model.UserName);
            if(user1.IsActive == false)
            {
                ModelState.AddModelError("", "Your account Deactivated.");
                return View();
            }
            if (user1 != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user1.Id))
                {
                    ModelState.AddModelError("", "You must have a confirmed email to log on.");
                    return View();
                }
            }
            Session["StaffName"] = user1.StaffName;
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, !model.RememberMe, shouldLockout: true);
            var user = await UserManager.FindAsync(model.UserName, model.Password);
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri + "Account/Login",
                UserName = User.Identity.Name,
                DETAIL = $"User {User.Identity.Name} has signed in successfully",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            objDbEntities.TBL_AUDIT.Add(audit);
            objDbEntities.SaveChanges();

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }
            else if (user.HasLoggedIn == false)
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
            //var user = await UserManager.FindByNameAsync(model.UserName);
            //bool? checkLoginFirstLogin = user.HasLoggedIn;
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {

            if (User.Identity.IsAuthenticated && User.IsInRole("System Admin"))
            {

                ViewBag.Name = new SelectList(context.Roles/*.Where(u => !u.Name.Contains("Admin"))*/
                                                     .ToList(), "Name", "Name");
                return View();
            }
            return RedirectToAction("Login");
            
        }

        //
        // POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    var user = new ApplicationUser();
        //    if(model.Password == null || model.Password == "")
        //    {
        //         user = new ApplicationUser
        //        {
        //            StaffName = model.StaffName,
        //            UserName = model.UserName,
        //            Email = model.Email,
        //            HasLoggedIn = false
        //        };
        //        var generator = new RandomPassword();
        //        string Pass = generator.RandomPasswordGenerator();
        //        model.Password = Pass + "Ab$";
        //    }
        //    else
        //    {
        //        user = new ApplicationUser
        //        {
        //            StaffName = model.StaffName,
        //            UserName = model.UserName,
        //            Email = model.Email,
        //            HasLoggedIn = false
        //        };
        //    }

        //    var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

        //            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
        //            // Send an email with this link
        //            // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //            await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
        //            //return RedirectToAction("Index", "Home");
        //            return View();
        //        }
        //        ViewBag.Name = new SelectList(context.Roles/*.Where(u => !u.Name.Contains("Admin"))*/
        //                                  .ToList(), "Name", "Name");
        //        AddErrors(result);
        //    //}

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}


        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public JsonResult Register(RegisterViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            string message = String.Empty;
            var user = new ApplicationUser();
            if (model.Password == null || model.Password == "")
            {
                user = new ApplicationUser
                {
                    StaffName = model.StaffName,
                    UserName = model.UserName,
                    Email = model.Email,
                    HasLoggedIn = false,
                    IsActive = true,
                };
                var generator = new RandomPassword();
                string Pass = generator.RandomPasswordGenerator();
                model.Password = Pass + "Ab$";
            }
            else
            {
                user = new ApplicationUser
                {
                    StaffName = model.StaffName,
                    UserName = model.UserName,
                    Email = model.Email,
                    HasLoggedIn = false,
                    IsActive = true
                };
            }

            var result =  UserManager.Create(user, model.Password);
            if (!result.Succeeded)
            {
                foreach(string err in result.Errors)
                {
                    message = err;
                }
            } 
            else
            {
                //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                var audit = new TBL_AUDIT()
                {
                    URL = Request.Url.AbsoluteUri,
                    UserName = User.Identity.Name,
                    DETAIL = $"New user {model.StaffName} has been profiled successfully by {User.Identity.Name}",
                    IPADDRESS = SendEmail.GetLocalIpAddress(),
                    DEVICENAME = SendEmail.GetDeviceName(),
                    OSNAME = SendEmail.FriendlyName(),
                    DATETIME = DateTime.UtcNow,
                    UserId = User.Identity.GetUserId()
                };
                objDbEntities.TBL_AUDIT.Add(audit);
                objDbEntities.SaveChanges();
                this.UserManager.AddToRole(user.Id, model.UserRoles);
                
                string code =  UserManager.GenerateEmailConfirmationToken(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                 UserManager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/AccountConfirmation.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{ConfirmationLink}", callbackUrl);
                body = body.Replace("{StaffName}", model.StaffName);
                body = body.Replace("{UserName}", model.UserName);
                body = body.Replace("{Password}", model.Password);
                bool IsSendEmail = SendEmail.EmailSend(model.Email, "Confirm your account", body, true);
                if (IsSendEmail)

                    //return RedirectToAction("Index", "Home");
                    //return View();
                    message = "Registered Successfully";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            ViewBag.Name = new SelectList(context.Roles/*.Where(u => !u.Name.Contains("Admin"))*/
                                      .ToList(), "Name", "Name");
            AddErrors(result);
            //}

            // If we got this far, something failed, redisplay form
            //return View(model);
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user1 = await UserManager.FindByNameAsync(model.UserName);
                var user = await UserManager.FindByEmailAsync(user1.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                string body = String.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/AccountReset.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{ResetLink}", callbackUrl);
                body = body.Replace("{UserName}", user1.StaffName);
                bool IsSendEmail = SendEmail.EmailSend(user1.Email, "Reset your account", body, true);
                if (IsSendEmail) { 
                    var audit = new TBL_AUDIT()
                    {
                        URL = Request.Url.AbsoluteUri + "Account/ForgotPassword",
                        UserName = User.Identity.Name,
                        DETAIL = $"User {user.UserName} has successfully performed change password operation",
                        IPADDRESS = SendEmail.GetLocalIpAddress(),
                        DEVICENAME = SendEmail.GetDeviceName(),
                        OSNAME = SendEmail.FriendlyName(),
                        DATETIME = DateTime.UtcNow,
                        UserId = User.Identity.GetUserId()
                    };
                objDbEntities.TBL_AUDIT.Add(audit);
                objDbEntities.SaveChanges();
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser user1 = await UserManager.FindByNameAsync(model.UserName);
            var user = await UserManager.FindByEmailAsync(user1.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            //var token = await UserManager.GeneratePasswordResetTokenAsync(user.Email);
            // Use passwordHash to add new password
            //user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
            //var result = await UserManager.UpdateAsync(user);
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
                //return RedirectToAction("Login", "Account");

            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri + "Account/LogOff",
                UserName = User.Identity.Name,
                DETAIL = $"User {User.Identity.Name} has signed out successfully",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            objDbEntities.TBL_AUDIT.Add(audit);
            objDbEntities.SaveChanges();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult CheckLogin(LoginVM loginVM)
        {
            SCBDBEntities db = new SCBDBEntities();
            string md5StringPassword = AppHelperMVC.GetMd5Hash(loginVM.Password);
            var dataItem = db.Tbl_Users.Where(x => x.Email == loginVM.Email && x.Password == md5StringPassword && x.IsActive == true).SingleOrDefault();
            //var dataItemForStaff = db.Tbl_BankStaff.Where(x => x.Email == loginVM.Email && x.IsActive == true).SingleOrDefault();
            bool isLogged = true;
            if (dataItem != null/* && dataItemForStaff != null*/)
            {
                Session["Email"] = dataItem.Email;
                isLogged = true;
            }
            else
            {
                isLogged = false;
                ViewBag.InvalidCredentials = "Wrong Email or Password";
                return View("Login");
            }

            if (isLogged)
            {
                //return View("/Home/Index");
                return RedirectToAction("Index", "Home");
            }
            return View("Login");
        }

        [HttpPost]
        public ActionResult ResetPsw(ResetPassVM reset)
        {
            SCBDBEntities db = new SCBDBEntities();
            if (!db.Tbl_Users.Any(e => e.Email == reset.Email))
            {
                ViewBag.Error = "Email " + reset.Email + " does not exists!";
                return View("ForgotPassword");
            }
            if (reset.Password != reset.ConfirmPassword)
            {
                ViewBag.PasswordError = "Password Mismatch";
                return View("ForgotPassword");
            }
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["SCBDBEntities"].ConnectionString;
            if (cnnString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(cnnString);
                cnnString = efBuilder.ProviderConnectionString;
            }
            string encPass = AppHelperMVC.GetMd5Hash(reset.Password);
            SqlConnection conn = new SqlConnection(cnnString);
            SqlCommand com = new SqlCommand("sp_resetPassword", conn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@userEmail", reset.Email);
            com.Parameters.AddWithValue("@userPassword", encPass);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();
            return View("Login");
        }
        public ActionResult Logout()
        {
            Session["StaffName"] = null;
            var audit = new TBL_AUDIT()
            {
                URL = Request.Url.AbsoluteUri,
                UserName = User.Identity.Name,
                DETAIL = $"User {User.Identity.Name} has signed out successfully",
                IPADDRESS = SendEmail.GetLocalIpAddress(),
                DEVICENAME = SendEmail.GetDeviceName(),
                OSNAME = SendEmail.FriendlyName(),
                DATETIME = DateTime.UtcNow,
                UserId = User.Identity.GetUserId()
            };
            objDbEntities.TBL_AUDIT.Add(audit);
            objDbEntities.SaveChanges();
            return RedirectToAction("Login");
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }


        }
        #endregion

      
    }
}