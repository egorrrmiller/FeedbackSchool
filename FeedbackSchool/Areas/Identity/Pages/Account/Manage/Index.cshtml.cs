using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FeedbackSchool.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly SignInManager<FeedbackSchoolUser> _signInManager;
    private readonly UserManager<FeedbackSchoolUser> _userManager;
    private FeedbackSchoolContext _context;

    public IndexModel(
        UserManager<FeedbackSchoolUser> userManager,
        SignInManager<FeedbackSchoolUser> signInManager, FeedbackSchoolContext db)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = db;
    }

    [Display(Name = "Имя пользователя")] public string Username { get; set; }

    [Display(Name = "ID Аккаунта")] public string IdUser { get; set; }

    [Display(Name = "Электронная почта")] public string EmailUser { get; set; }

    [TempData] public string StatusMessage { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    private async Task LoadAsync(FeedbackSchoolUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var idUser = await _userManager.GetUserIdAsync(user);
        var emailUser = await _userManager.GetEmailAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);


        Username = userName;
        IdUser = idUser;
        EmailUser = emailUser;

        Input = new InputModel
        {
            NewUsername = userName,
            PhoneNumber = phoneNumber
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        /*if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }*/

        var userName = await _userManager.GetUserNameAsync(user);
        if (Input.NewUsername != userName)
        {
            var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.NewUsername);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Неожиданная ошибка при попытке установить новое имя пользователя.";
                return RedirectToPage();
            }
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Неожиданная ошибка при попытке установить номер телефона.";
                return RedirectToPage();
            }
        }

        await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Ваш профиль был обновлен";
        return RedirectToPage();
    }

    public class InputModel
    {
        [Display(Name = "Сменить имя пользователя")]
        public string NewUsername { get; set; }

        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}