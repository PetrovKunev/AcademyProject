using Academy.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Web.Pages;

public class CoursesModel : PageModel
{
    private readonly ICourseService _courseService;

    public CoursesModel(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public void OnGet()
    {
        // The page will load courses via HTMX
    }
} 