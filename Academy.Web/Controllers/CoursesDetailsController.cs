using Academy.Application.Services;
using Academy.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Controllers;

public class CoursesDetailsController : Controller
{
    private readonly ICourseService _courseService;

    public CoursesDetailsController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("Courses/Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            // Redirect to specific course pages based on title
            if (course.Title.Contains("Програмиране - 5 клас"))
            {
                return RedirectToPage("/Courses/Programming5");
            }
            else if (course.Title.Contains("Програмиране - 6 клас"))
            {
                return RedirectToPage("/Courses/Programming6");
            }
            else if (course.Title.Contains("Програмиране - 7 клас"))
            {
                return RedirectToPage("/Courses/Programming7");
            }
            else if (course.Title.Contains("Математика - 5 клас"))
            {
                return RedirectToPage("/Courses/Math5");
            }
            else if (course.Title.Contains("Математика - 6 клас"))
            {
                return RedirectToPage("/Courses/Math6");
            }
            else if (course.Title.Contains("Математика - 7 клас"))
            {
                return RedirectToPage("/Courses/Math7");
            }
            else if (course.Title.Contains("БЕЛ - 5 клас"))
            {
                return RedirectToPage("/Courses/BEL5");
            }
            else if (course.Title.Contains("БЕЛ - 6 клас"))
            {
                return RedirectToPage("/Courses/BEL6");
            }
            else if (course.Title.Contains("БЕЛ - 7 клас"))
            {
                return RedirectToPage("/Courses/BEL7");
            }
            else
            {
                // For other courses, redirect to general courses page
                return RedirectToPage("/Courses");
            }
        }
        catch (Exception)
        {
            return RedirectToPage("/Courses");
        }
    }
} 