using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FeedbackSchool.Areas.Identity.Pages.Account.Manage;

public class ResetAuthenticatorModel : PageModel
{
    private readonly SignInManager<FeedbackSchoolUser> _signInManager;

    private ILogger<ResetAuthenticatorModel> _logger;

    private UserManager<FeedbackSchoolUser> _userManager;

    public ResetAuthenticatorModel(UserManager<FeedbackSchoolUser> userManager,
                                   SignInManager<FeedbackSchoolUser> signInManager,
                                   ILogger<ResetAuthenticatorModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

        await _signInManager.RefreshSignInAsync(user);

        StatusMessage =
            "Ваш ключ аутентификатора был сброшен, вам нужно будет настроить аутентификатор с помощью нового ключа.";

        return RedirectToPage("./EnableAuthenticator");
    }
}