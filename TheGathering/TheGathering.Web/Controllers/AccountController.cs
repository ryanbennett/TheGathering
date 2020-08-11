using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SendGrid;
using SendGrid.Helpers.Mail;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels.Account;

namespace TheGathering.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);



            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("VolunteerCalendar", "VolunteerEvent", null);
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
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
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
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AccountRegistrationViewModel model)
        {

            DateTime local = model.Birthday.ToUniversalTime();
            DateTime server = DateTime.Now.ToUniversalTime();
            var age = server.Subtract(local);
            if (local.Year < 1900)
            {
                ModelState.AddModelError("Birthday", "Birthday date is out of range");
            }
            if (local >= server)
            {
                ModelState.AddModelError("Birthday", "Birthday date does not exist");
            }
            if (age.TotalDays / 365 < 18)
            {
                ModelState.AddModelError("Birthday", "Volunteer must be older than 18");
            }
            if (model.FirstName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("FirstName", "First name cannot contain numbers");
            }
            if (model.LastName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("LastName", "Last name cannot contain numbers");
            }
            if (model.Email.Contains('.') == false)
            {
                ModelState.AddModelError("Email", "Email must contain a period");
            }
            if (model.Password.Any(char.IsDigit) == false)
            {
                ModelState.AddModelError("Password", "Password must contain numbers");
            }
            if (model.Password.Any(char.IsUpper) == false)
            {
                ModelState.AddModelError("Password", "Password must contain an uppercase letter");
            }
            if (model.Password.Any(char.IsLower) == false)
            {
                ModelState.AddModelError("Password", "Password must contain a lowercase letter");
            }
            if (!model.Password.Contains("!") && !model.Password.Contains("@") && !model.Password.Contains("#") && !model.Password.Contains("$") & !model.Password.Contains("%") && !model.Password.Contains("^") && !model.Password.Contains("&") && !model.Password.Contains("*"))
            {
                ModelState.AddModelError("Password", "Password must contain a symbol or special character");
            }


            if (ModelState.IsValid)

            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "volunteer");

                    var VolunteerService = new VolunteerService();

                    Volunteer volunteer = new Volunteer();
                    volunteer.FirstName = model.FirstName;
                    volunteer.LastName = model.LastName;
                    volunteer.Birthday = model.Birthday;
                    volunteer.PhoneNumber = model.PhoneNumber;
                    volunteer.InterestInLeadership = false;
                    volunteer.SignUpForNewsLetter = model.SignUpForNewsLetter;
                    volunteer.ApplicationUserId = user.Id;
                    volunteer.Email = model.Email;

                    VolunteerService.Create(volunteer);

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);


                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                       new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);


                    String subject = "The Gathering Registration Confirmation";
                    String plainText = "Hello " + model.FirstName + ", Thank you for creating an account with The Gathering! Our volunteers are the backbone of our organization.We are dedicated toFeeding the Hungry & Keeping Hearts Full and we look forward to seeing you soon.";
                    String htmlText = "Hello " + model.FirstName + ", Thank you for creating an account with The Gathering! Our volunteers are the backbone of our organization.We are dedicated to Feeding the Hungry & Keeping Hearts Full and we look forward to seeing you soon.<br/> <a href='" + callbackUrl + "' target='_new'>Click here to confirm your account</a> <br/> <img src='https://trello-attachments.s3.amazonaws.com/5ec81f7ae324c641265eab5e/5f046a07b1869070763f0493/3127105983ac3dd06e02da13afa54a02/The_Gathering_F2_Full_Color_Black.png' width='600px' style='pointer-events: none; display: block; margin-left: auto; margin-right: auto; width: 50%;'>";

                    await ConfirmationEmail(model.FirstName, model.Email, subject, plainText, htmlText);
                    
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    return RedirectToAction("VolunteerCalendar", "VolunteerEvent", null);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult GroupRegister()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GroupRegister(GroupRegistrationViewModel model)
        {

            DateTime local = model.LeaderBirthday.ToUniversalTime();
            DateTime server = DateTime.Now.ToUniversalTime();
            var age = server.Subtract(local);
            if (local.Year < 1900)
            {
                ModelState.AddModelError("Birthday", "Birthday date is out of range");
            }
            if (local >= server)
            {
                ModelState.AddModelError("Birthday", "Birthday date does not exist");
            }
            if (age.TotalDays / 365 < 18)
            {
                ModelState.AddModelError("Birthday", "Volunteer must be older than 18");
            }
            if (model.LeaderFirstName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("FirstName", "First name cannot contain numbers");
            }
            if (model.LeaderLastName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("LastName", "Last name cannot contain numbers");
            }
            if (model.Email.Contains('.') == false)
            {
                ModelState.AddModelError("Email", "Email must contain a period");
            }
            if (model.Password.Any(char.IsDigit) == false)
            {
                ModelState.AddModelError("Password", "Password must contain numbers");
            }
            if (model.Password.Any(char.IsUpper) == false)
            {
                ModelState.AddModelError("Password", "Password must contain an uppercase letter");
            }
            if (model.Password.Any(char.IsLower) == false)
            {
                ModelState.AddModelError("Password", "Password must contain a lowercase letter");
            }
            if (!model.Password.Contains("!") && !model.Password.Contains("@") && !model.Password.Contains("#") && !model.Password.Contains("$") & !model.Password.Contains("%") && !model.Password.Contains("^") && !model.Password.Contains("&") && !model.Password.Contains("*"))
            {
                ModelState.AddModelError("Password", "Password must contain a symbol or special character");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id,"groupleader");
                    var VolunteerGroupService = new VolunteerGroupService();

                    VolunteerGroupLeader volunteerLeader = new VolunteerGroupLeader();
                    volunteerLeader.LeaderFirstName = model.LeaderFirstName;
                    volunteerLeader.LeaderLastName = model.LeaderLastName;
                    volunteerLeader.LeaderBirthday = model.LeaderBirthday;
                    volunteerLeader.LeaderPhoneNumber = model.LeaderPhoneNumber;
                    volunteerLeader.SignUpForNewsLetter = model.SignUpForNewsLetter;
                    volunteerLeader.ApplicationUserId = user.Id;
                    volunteerLeader.LeaderEmail = model.Email;
                    volunteerLeader.GroupName = model.GroupName;
                    volunteerLeader.TotalGroupMembers = model.TotalGroupMembers;

                    VolunteerGroupService.CreateLeader(volunteerLeader);

                    String subject = "The Gathering Registration Confirmation";
                    String plainText = "Hello" + model.LeaderFirstName + ", Thank you for creating an account with The Gathering! Our volunteers are the backbone of our organization.We are dedicated to Feeding the Hungry & Keeping Hearts Full and we look forward to seeing you soon.";
                    String htmlText = "Hello" + model.LeaderFirstName + ", Thank you for creating an account with The Gathering! Our volunteers are the backbone of our organization.We are dedicated to Feeding the Hungry & Keeping Hearts Full and we look forward to seeing you soon. <br/> <img src='https://trello-attachments.s3.amazonaws.com/5ec81f7ae324c641265eab5e/5f046a07b1869070763f0493/3127105983ac3dd06e02da13afa54a02/The_Gathering_F2_Full_Color_Black.png' width='600px' style='pointer-events: none; display: block; margin-left: auto; margin-right: auto; width: 50%;'>";

                    await ConfirmationEmail(model.LeaderFirstName, model.Email, subject, plainText, htmlText);


                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");



                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [AllowAnonymous]
        public ActionResult AdminRegister()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminResgister(AccountRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var VolunteerService = new VolunteerService();

                    Volunteer volunteer = new Volunteer();
                    volunteer.FirstName = model.FirstName;
                    volunteer.LastName = model.LastName;
                    volunteer.Birthday = model.Birthday;
                    volunteer.PhoneNumber = model.PhoneNumber;
                    volunteer.InterestInLeadership = model.InterestInLeadership;
                    volunteer.SignUpForNewsLetter = model.SignUpForNewsLetter;
                    volunteer.ApplicationUserId = user.Id;
                    volunteer.Email = model.Email;

                    VolunteerService.Create(volunteer);


                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                var apiKey = WebConfigurationManager.AppSettings["SendGridEnvironmentalKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("21ahmeda@elmbrookstudents.org", "The Gathering");
                var subject = "The Gathering Password Reset";
                var to = new EmailAddress(model.Email);
                var plainTextContent = " Please reset your password by clicking the link" + callbackUrl;
                var htmlContent = "Please reset your password by clicking <br/> <a href='" + callbackUrl + "' target='_new'>Here</a> <br/>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
               
               // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
               // Send an email with this link
               // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
               // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
               // UserManager.EmailService = new EmailService();
               // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
               // return RedirectToAction("ForgotPasswordConfirmation", "Account");
               
                
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
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
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
            return RedirectToAction("Index", "Home");
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