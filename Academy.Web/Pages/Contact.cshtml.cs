using Academy.Application.Services;
using Academy.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Academy.Web.Services;

namespace Academy.Web.Pages;

public class ContactModel : PageModel
{
    private readonly IContactService _contactService;
    private readonly EmailService _emailService;

    public ContactModel(IContactService contactService, EmailService emailService)
    {
        _contactService = contactService;
        _emailService = emailService;
    }

    [BindProperty]
    public ContactMessage ContactMessage { get; set; } = new();

    public void OnGet()
    {
        // Initialize empty contact message
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            ContactMessage.CreatedAt = DateTime.UtcNow;
            await _contactService.SendMessageAsync(ContactMessage);
            try
            {
                await _emailService.SendContactEmailAsync(ContactMessage);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Възникна грешка при изпращане на имейла: " + ex.Message;
                ModelState.AddModelError("", "Грешка при изпращане на имейла: " + ex.Message);
                return Page();
            }
            TempData["SuccessMessage"] = "Съобщението е изпратено успешно! Ще се свържем с вас скоро.";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Възникна грешка при изпращане на съобщението. Моля, опитайте отново по-късно.";
            ModelState.AddModelError("", "Грешка при изпращане на съобщението: " + ex.Message);
            return Page();
        }
    }
} 