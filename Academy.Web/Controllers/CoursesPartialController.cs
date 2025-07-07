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
        var allowedTitles = new[]
        {
            "Математика - 5 клас",
            "Математика - 6 клас",
            "Математика - 7 клас /Подготовка за НВО/",
            "БЕЛ - 5 клас",
            "БЕЛ - 6 клас",
            "БЕЛ - 7 клас /Подготовка за НВО/"
        };
        var allowedCategories = new[] { "Математика", "БЕЛ" };
        var courses = (await _courseService.GetActiveCoursesAsync())
            .Where(c => allowedTitles.Contains(c.Title) && allowedCategories.Contains(c.Category))
            .OrderBy(c => c.Category)
            .ThenBy(c => c.Title)
            .ToList();
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
        return PartialView("~/Pages/Shared/_CoursesPartial.cshtml", courses);
    }
} 