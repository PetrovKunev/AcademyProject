using Academy.Core.Interfaces;
using Academy.Core.Models;

namespace Academy.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        return await _courseRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
    {
        return await _courseRepository.GetActiveAsync();
    }

    public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be null or empty", nameof(category));

        return await _courseRepository.GetByCategoryAsync(category);
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Course ID must be greater than 0", nameof(id));

        return await _courseRepository.GetByIdAsync(id);
    }

    public async Task<Course> CreateCourseAsync(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        ValidateCourse(course);
        
        course.CreatedAt = DateTime.UtcNow;
        course.IsActive = true;
        
        return await _courseRepository.AddAsync(course);
    }

    public async Task<Course> UpdateCourseAsync(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        if (course.Id <= 0)
            throw new ArgumentException("Course ID must be greater than 0", nameof(course));

        ValidateCourse(course);
        
        var existingCourse = await _courseRepository.GetByIdAsync(course.Id);
        if (existingCourse == null)
            throw new InvalidOperationException($"Course with ID {course.Id} not found");

        course.UpdatedAt = DateTime.UtcNow;
        course.CreatedAt = existingCourse.CreatedAt;
        
        return await _courseRepository.UpdateAsync(course);
    }

    public async Task DeleteCourseAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Course ID must be greater than 0", nameof(id));

        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new InvalidOperationException($"Course with ID {id} not found");

        await _courseRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        var courses = await _courseRepository.GetActiveAsync();
        return courses.Select(c => c.Category).Distinct().OrderBy(c => c);
    }

    public async Task<IEnumerable<string>> GetLevelsAsync()
    {
        var courses = await _courseRepository.GetActiveAsync();
        return courses.Select(c => c.Level).Distinct().OrderBy(c => c);
    }

    private static void ValidateCourse(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.Title))
            throw new ArgumentException("Course title is required");

        if (string.IsNullOrWhiteSpace(course.Description))
            throw new ArgumentException("Course description is required");

        if (string.IsNullOrWhiteSpace(course.Category))
            throw new ArgumentException("Course category is required");

        if (string.IsNullOrWhiteSpace(course.Level))
            throw new ArgumentException("Course level is required");

        if (course.Duration <= 0)
            throw new ArgumentException("Course duration must be greater than 0");

        if (course.Price < 0)
            throw new ArgumentException("Course price cannot be negative");
    }
} 