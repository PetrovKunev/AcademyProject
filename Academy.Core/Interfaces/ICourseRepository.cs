using Academy.Core.Models;

namespace Academy.Core.Interfaces;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<IEnumerable<Course>> GetActiveAsync();
    Task<IEnumerable<Course>> GetByCategoryAsync(string category);
    Task<Course?> GetByIdAsync(int id);
    Task<Course> AddAsync(Course course);
    Task<Course> UpdateAsync(Course course);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
} 