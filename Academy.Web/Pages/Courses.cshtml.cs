using Academy.Application.Services;
using Academy.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Web.Pages;

public class CoursesModel : PageModel
{
    private readonly ICourseService _courseService;

    public CoursesModel(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task OnGetAsync()
    {
        // Load courses directly in the page model
        try
        {
            var courses = await _courseService.GetActiveCoursesAsync();
            ViewData["Courses"] = courses;
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = "Грешка при зареждане на курсовете: " + ex.Message;
        }
    }
} 