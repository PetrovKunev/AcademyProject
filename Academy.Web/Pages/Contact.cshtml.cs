using Academy.Application.Services;
using Academy.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Web.Pages;

public class ContactModel : PageModel
{
    private readonly IContactService _contactService;

    public ContactModel(IContactService contactService)
    {
        _contactService = contactService;
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