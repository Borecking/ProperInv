namespace ProperTax.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 6)]
            public string Password { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 6)]
            [Compare("Password")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };

                // Tworzenie użytkownika
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // Zalogowanie użytkownika po rejestracji
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Upewnienie się, że rola "Bookkeeper" istnieje
                    if (!await _roleManager.RoleExistsAsync("Bookkeeper"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Bookkeeper"));
                    }

                    // Przypisanie roli Bookkeeper do użytkownika
                    await _userManager.AddToRoleAsync(user, "Bookkeeper");

                    return RedirectToPage("/Index"); // Po rejestracji przekierowanie na stronę główną
                }

                // Jeżeli rejestracja nie powiedzie się, dodanie błędów
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page(); // Jeśli wystąpią błędy, wróć do formularza rejestracji
        }
    }
}
