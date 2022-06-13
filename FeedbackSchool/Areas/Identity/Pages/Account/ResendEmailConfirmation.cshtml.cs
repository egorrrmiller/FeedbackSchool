using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FeedbackSchool.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResendEmailConfirmationModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<FeedbackSchoolUser> _userManager;

    public ResendEmailConfirmationModel(UserManager<FeedbackSchoolUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [BindProperty] public InputModel Input { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Ссылка для подтверждения отправлена. Пожалуйста, проверьте почту.");
            return Page();
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            null,
            new {userId, code},
            Request.Scheme);
        await _emailSender.SendEmailAsync(
            Input.Email,
            "Подтверждение почты",
            $"Чтобы подтвердить ваш аккаунт, <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>.");

        ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
        return Page();
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
    }
}