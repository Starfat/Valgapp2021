using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Valgapplikasjon.Areas.Identity;

namespace Valgapplikasjon.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ValgapplikasjonUser> _signInManager;
        private readonly UserManager<ValgapplikasjonUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ValgapplikasjonUser> userManager,
            SignInManager<ValgapplikasjonUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Fornavn må fylles ut")]
            [DataType(DataType.Text)]
            [Display(Name = "Fornavn")]
            public string Fornavn { get; set; }

            [Required(ErrorMessage = "Etternavn må fylles ut")]
            [DataType(DataType.Text)]
            [Display(Name = "Etternavn")]
            public string Etternavn { get; set; }

            [Required(ErrorMessage = "Epost må fylles ut")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            // Validering av passord er gjort egendefinert med RegEx. Standardvalidering fra Identity er satt til false.
            [Required(ErrorMessage = "Passord må fylles ut")]
            [RegularExpression(@"^^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,100}$", ErrorMessage = "Minimum 8 karakterer | Store og små bokstaver | Minimum ett tall")]
            [DataType(DataType.Password)]
           // [Display(Name = "Password")]
            public string Password { get; set; }
        
            [Required(ErrorMessage = "Vennligst bekreft passord")]
            [DataType(DataType.Password)]
            [Display(Name = "Bekreft passord")]
            [Compare("Password", ErrorMessage = "Passord stemmer ikke med oppgitt passord")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ValgapplikasjonUser { UserName = Input.Email, Email = Input.Email, Fornavn = Input.Fornavn, Etternavn = Input.Etternavn };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Ny bruker blir assignet rollen "Bruker"
                    await _userManager.AddToRoleAsync(user, Enums.Roles.Bruker.ToString());

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
