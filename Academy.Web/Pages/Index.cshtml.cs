using Academy.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ICourseService _courseService;

    public IndexModel(ILogger<IndexModel> logger, ICourseService courseService)
    {
        _logger = logger;
        _courseService = courseService;
    }

    public void OnGet()
    {
        // The page will load courses via HTMX
    }
}
