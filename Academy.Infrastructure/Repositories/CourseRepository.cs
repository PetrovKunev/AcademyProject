using Academy.Core.Interfaces;
using Academy.Core.Models;
using Academy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Academy.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AcademyDbContext _context;

    public CourseRepository(AcademyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetActiveAsync()
    {
        return await _context.Courses
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByCategoryAsync(string category)
    {
        return await _context.Courses
            .Where(c => c.Category == category && c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses.FindAsync(id);
    }

    public async Task<Course> AddAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course> UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task DeleteAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Courses.AnyAsync(c => c.Id == id);
    }
} 