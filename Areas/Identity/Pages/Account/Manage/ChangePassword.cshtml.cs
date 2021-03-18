using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Valgapplikasjon.Areas.Identity;
namespace Valgapplikasjon.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ValgapplikasjonUser> _userManager;
        private readonly SignInManager<ValgapplikasjonUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<ValgapplikasjonUser> userManager,
            SignInManager<ValgapplikasjonUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Feltet må fylles ut")]
            [DataType(DataType.Password)]
            [Display(Name = "Nåværende passord")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "Feltet må fylles ut")]
            [RegularExpression(@"^^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,100}$", ErrorMessage = "Minimum 8 karakterer | Store og små bokstaver | Minimum ett tall")]
            [DataType(DataType.Password)]
            [Display(Name = "Nytt passord")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bekreft nytt passord")]
            [Compare("NewPassword", ErrorMessage = "Bekreftet passord stemmer ikke med nytt passord")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                IdentityError customDescription = new IdentityError(); // lager et objekt av IdentityError klassen

                     ModelState.AddModelError(string.Empty, customDescription.Description = "Feil passord"); // legger til egen feilmld med nytt objekt

                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Passordet ditt er nå endret";

            return RedirectToPage();
        }
    }
}
