using System.Text;
using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FeedbackSchool.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ConfirmEmailChangeModel : PageModel
{
    private readonly SignInManager<FeedbackSchoolUser> _signInManager;

    private readonly UserManager<FeedbackSchoolUser> _userManager;

    public ConfirmEmailChangeModel(UserManager<FeedbackSchoolUser> userManager,
                                   SignInManager<FeedbackSchoolUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
    {
        if (userId == null || email == null || code == null)
        {
            return RedirectToPage("/Index");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ChangeEmailAsync(user, email, code);

        if (!result.Succeeded)
        {
            StatusMessage = "Ошибка изменения электронной почты.";

            return Page();
        }

        // In our UI email and user name are one and the same, so when we update the email
        // we need to update the user name.
        var setUserNameResult = await _userManager.SetUserNameAsync(user, email);

        if (!setUserNameResult.Succeeded)
        {
            StatusMessage = "Ошибка изменения имени пользователя.";

            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Спасибо за подтверждение электронной почты.";

        return Page();
    }
}