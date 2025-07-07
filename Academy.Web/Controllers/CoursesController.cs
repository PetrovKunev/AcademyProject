using Academy.Application.Services;
using Academy.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Academy.Web.Controllers;

[Route("api/[controller]")]
public class CoursesController : Controller
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        try
        {
            var courses = await _courseService.GetActiveCoursesAsync();
            return Ok(courses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving courses", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(int id)
    {
        try
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { message = $"Course with ID {id} not found" });
            }

            return Ok(course);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the course", error = ex.Message });
        }
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByCategory(string category)
    {
        try
        {
            var courses = await _courseService.GetCoursesByCategoryAsync(category);
            return Ok(courses);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving courses by category", error = ex.Message });
        }
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        try
        {
            var categories = await _courseService.GetCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving categories", error = ex.Message });
        }
    }

    [HttpGet("levels")]
    public async Task<ActionResult<IEnumerable<string>>> GetLevels()
    {
        try
        {
            var levels = await _courseService.GetLevelsAsync();
            return Ok(levels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving levels", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Course>> CreateCourse(Course course)
    {
        try
        {
            var createdCourse = await _courseService.CreateCourseAsync(course);
            return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.Id }, createdCourse);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the course", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, Course course)
    {
        try
        {
            if (id != course.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var updatedCourse = await _courseService.UpdateCourseAsync(course);
            return Ok(updatedCourse);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the course", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        try
        {
            await _courseService.DeleteCourseAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the course", error = ex.Message });
        }
    }

    [HttpGet("featuredpartial")]
    public async Task<IActionResult> FeaturedPartial()
    {
        var courses = (await _courseService.GetActiveCoursesAsync())
            .OrderByDescending(c => c.CreatedAt)
            .Take(3)
            .ToList();
        return PartialView("~/Pages/Shared/_CoursesPartial.cshtml", courses);
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetFilteredCourses([FromQuery] string? category, [FromQuery] string? level, [FromQuery] string? sort)
    {
        try
        {
            var courses = await _courseService.GetActiveCoursesAsync();
            
            // Apply category filter
            if (!string.IsNullOrEmpty(category))
            {
                courses = courses.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }
            
            // Apply level filter
            if (!string.IsNullOrEmpty(level))
            {
                courses = courses.Where(c => c.Level.Equals(level, StringComparison.OrdinalIgnoreCase));
            }
            
            // Apply sorting
            courses = sort?.ToLower() switch
            {
                "newest" => courses.OrderByDescending(c => c.CreatedAt),
                "oldest" => courses.OrderBy(c => c.CreatedAt),
                "price-low" => courses.OrderBy(c => c.Price),
                "price-high" => courses.OrderByDescending(c => c.Price),
                _ => courses.OrderByDescending(c => c.CreatedAt) // default sorting
            };
            
            return PartialView("~/Pages/Shared/_CoursesPartial.cshtml", courses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while filtering courses", error = ex.Message });
        }
    }
} 