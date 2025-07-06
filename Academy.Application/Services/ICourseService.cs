using Academy.Core.Models;

namespace Academy.Application.Services;

public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllCoursesAsync();
    Task<IEnumerable<Course>> GetActiveCoursesAsync();
    Task<IEnumerable<Course>> GetCoursesByCategoryAsync(string category);
    Task<Course?> GetCourseByIdAsync(int id);
    Task<Course> CreateCourseAsync(Course course);
    Task<Course> UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(int id);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<IEnumerable<string>> GetLevelsAsync();
} 