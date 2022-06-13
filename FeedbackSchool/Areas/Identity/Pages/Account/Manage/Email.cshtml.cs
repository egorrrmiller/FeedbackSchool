using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FeedbackSchool.Areas.Identity.Pages.Account.Manage;

public class EmailModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<FeedbackSchoolUser> _signInManager;
    private readonly UserManager<FeedbackSchoolUser> _userManager;

    public EmailModel(
        UserManager<FeedbackSchoolUser> userManager,
        SignInManager<FeedbackSchoolUser> signInManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public string Username { get; set; }

    [Display(Name = "Электронная почта")] public string Email { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [TempData] public string StatusMessage { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    private async Task LoadAsync(FeedbackSchoolUser user)
    {
        var email = await _userManager.GetEmailAsync(user);
        Email = email;

        Input = new InputModel
        {
            NewEmail = email
        };

        IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostChangeEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var email = await _userManager.GetEmailAsync(user);
        if (Input.NewEmail != email)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmailChange",
                null,
                new {userId, email = Input.NewEmail, code},
                Request.Scheme);
            await _emailSender.SendEmailAsync(Input.NewEmail, "Подтверждение почты",
                $"Чтобы подтвердить ваш аккаунт, <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>.");

            StatusMessage =
                "Ссылка подтверждения для изменения отправленного письма. Пожалуйста, проверьте свою электронную почту.";
            return RedirectToPage();
        }

        StatusMessage = "Ваша электронная почта не изменилась.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var email = await _userManager.GetEmailAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            null,
            new {area = "Identity", userId, code},
            Request.Scheme);
        await _emailSender.SendEmailAsync(email, "Подтверждение почты",
            $"Чтобы подтвердить ваш аккаунт, <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>.");

        StatusMessage = "Отправлено проверочное письмо. Пожалуйста, проверьте свою электронную почту.";
        return RedirectToPage();
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Новая электронная почта")]
        public string NewEmail { get; set; }
    }
}