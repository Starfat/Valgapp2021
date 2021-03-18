using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valgapplikasjon.Areas.Identity;

namespace Valgapplikasjon.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ValgapplikasjonUser> _userManager;
        private readonly SignInManager<ValgapplikasjonUser> _signInManager;

        public IndexModel(
            UserManager<ValgapplikasjonUser> userManager,
            SignInManager<ValgapplikasjonUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Display(Name = "Brukernavn")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [PersonalData]
			[Display(Name = "Telefonnummer")]
            public string PhoneNumber { get; set; }

            [PersonalData]
            [Display(Name = "Fornavn")]
            public String Fornavn { get; set; }

            [PersonalData]
            [Display(Name = "Etternavn")]
            public String Etternavn { get; set; }
 
            [PersonalData]
            [Display(Name = "Bio Overskrift")]

            public String BioTittel { get; set; }
            [PersonalData]
			[Display(Name = "Bio")]

			public String Bio { get; set; }

            [Display(Name = "Profilbilde")]
            public byte[] Profilbilde { get; set; }
        }

        private async Task LoadAsync(ValgapplikasjonUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.Fornavn;
            var lastName = user.Etternavn;
            var profilePicture = user.Profilbilde;
            var bioTittel = user.BioTittel;
            var bio = user.Bio;


            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Fornavn = firstName,
                Etternavn = lastName,
                Profilbilde = profilePicture,
                BioTittel = bioTittel,
                Bio = bio
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var firstName = user.Fornavn;
            var lastName = user.Etternavn;

            if (Input.Fornavn != firstName)
            {
                user.Fornavn = Input.Fornavn;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Etternavn != lastName)
            {
                user.Etternavn = Input.Etternavn;
                await _userManager.UpdateAsync(user);
            }

           if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    user.Profilbilde = dataStream.ToArray();
                }
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profilen din har blitt oppdatert";
            return RedirectToPage();
        }
    }
}
