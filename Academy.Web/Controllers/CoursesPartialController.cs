using Academy.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Controllers;

[Route("CoursesPartial")]
public class CoursesPartialController : Controller
{
    private readonly ICourseService _courseService;

    public CoursesPartialController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("Featured")]
    public async Task<IActionResult> Featured()
    {
        var courses = (await _courseService.GetActiveCoursesAsync())
            .Where(c => c.Category == "Математика")
            .OrderBy(c => c.Title)
            .ToList();
        ViewData["ButtonType"] = "pricing";
        return PartialView("~/Pages/Shared/_CoursesPartial.cshtml", courses);
    }

    [HttpGet("Filtered")]
    public async Task<IActionResult> Filtered(string? category, string? level, string? sort)
    {
        var courses = await _courseService.GetActiveCoursesAsync();
        if (!string.IsNullOrEmpty(category))
            courses = courses.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(level))
            courses = courses.Where(c => c.Level.Equals(level, StringComparison.OrdinalIgnoreCase));
        courses = sort?.ToLower() switch
        {
            "newest" => courses.OrderByDescending(c => c.CreatedAt),
            "oldest" => courses.OrderBy(c => c.CreatedAt),
            "price-low" => courses.OrderBy(c => c.Price),
            "price-high" => courses.OrderByDescending(c => c.Price),
            _ => courses.OrderByDescending(c => c.CreatedAt)
        };
        ViewData["ButtonType"] = "details";
        return PartialView("~/Pages/Shared/_CoursesPartial.cshtml", courses);
    }

    [HttpGet("FeaturedMath")]
    public async Task<IActionResult> FeaturedMath()
    {
        var courses = (await _courseService.GetActiveCoursesAsync())
            .Where(c => c.Category == "Математика")
            .OrderBy(c => c.Title)
            .ToList();
        return PartialView("~/Pages/Shared/_FeaturedMathCoursesPartial.cshtml", courses);
    }
} 