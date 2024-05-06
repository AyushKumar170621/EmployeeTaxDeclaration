// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EmployeeTaxDeclaration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeTaxDeclaration.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required, NotNull]
            [Display(Name = "Enter your first name.")]
            [StringLength(50, ErrorMessage = "First name length should not exceed 50.")]
            public string FirstName { get; set; } = string.Empty;

            [Required, NotNull]
            [Display(Name = "Enter your last name.")]
            [StringLength(50, ErrorMessage = "Last name length should not exceed 50.")]
            public string LastName { get; set; } = string.Empty;

            [Required, NotNull]
            [Display(Name = "Enter your PAN number.")]
            [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]$", ErrorMessage = "Please enter a valid PAN number.")]
            public string PanNumber { get; set; }

            [Required, NotNull]
            [Display(Name = "Enter your Social Security number.")]
            [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Please enter a valid Social Security number (XXX-XX-XXXX).")]
            public string SocialSecurityNumber { get; set; }

            [Display(Name = "Choose Profile.")]
            public IFormFile Profile { get; set; }

            public string ProPath { get; set; }

            [Required, NotNull]
            [Display(Name = "Enter your address.")]
            [StringLength(150, ErrorMessage = "Address should not exceed 150 characters.")]
            public string Address { get; set; }

            [Required, NotNull]
            [Display(Name = "Pin Code")]
            [RegularExpression(@"^\d{6}$", ErrorMessage = "Please enter a valid 6-digit PIN code.")]
            public string ZipCode { get; set; }
            
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var address = user.Address;
            var zipcode = user.ZipCode;
            var pan = user.PanNumber;
            var ssn = user.SocialSecurityNumber;
            var imgPath = user.ProfilePic;
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                ZipCode = zipcode,
                PanNumber = pan,
                SocialSecurityNumber = ssn,
                ProPath = imgPath
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
            
            if (Input.PhoneNumber != phoneNumber || user.FirstName != Input.FirstName ||
    user.LastName != Input.LastName ||
    user.PanNumber != Input.PanNumber ||
    user.SocialSecurityNumber != Input.SocialSecurityNumber ||
    user.ZipCode != Input.ZipCode ||
    user.Address != Input.Address ||
    user.ProfilePic != Input.Profile.FileName)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.PanNumber = Input.PanNumber;
                user.SocialSecurityNumber = Input.SocialSecurityNumber;
                user.ZipCode = Input.ZipCode;
                user.Address = Input.Address;
                var image = Input.Profile;
                if (image != null)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "images", image.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    user.ProfilePic = image.FileName;
                }
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
