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
public class ForgotPasswordModel : PageModel
{
    private readonly IEmailSender _emailSender;

    private readonly UserManager<FeedbackSchoolUser> _userManager;

    public ForgotPasswordModel(UserManager<FeedbackSchoolUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))

                // Don't reveal that the user does not exist or is not confirmed
            {
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page("/Account/ResetPassword",
                null,
                new
                {
                    area = "Identity",
                    code
                },
                Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email,
                "Сброс пароля",
                $"Чтобы сбросить пароль от вашего аккаунта, <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>. <br/> Если вы ничего не запрашивали, просто проигнорируйте это сообщение");

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

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