using Academy.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Web.Pages;

public class ContactModel : PageModel
{
    private readonly IContactService _contactService;

    public ContactModel(IContactService contactService)
    {
        _contactService = contactService;
    }

    public void OnGet()
    {
        // The page will handle form submission via HTMX
    }
} 