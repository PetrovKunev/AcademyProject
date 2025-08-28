using Academy.Core.Models;

namespace Academy.Core.Interfaces;

public interface IBlogPostRepository
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    Task<IEnumerable<BlogPost>> GetPublishedAsync();
    Task<BlogPost?> GetByIdAsync(int id);
    Task<BlogPost?> GetBySlugAsync(string slug);
    Task<IEnumerable<BlogPost>> GetByCategoryAsync(string category);
    Task<IEnumerable<BlogPost>> GetByTagAsync(string tag);
    Task<IEnumerable<BlogPost>> SearchAsync(string searchTerm);
    Task<IEnumerable<BlogPost>> GetRecentAsync(int count = 5);
    Task<IEnumerable<BlogPost>> GetPopularAsync(int count = 5);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<IEnumerable<string>> GetTagsAsync();
    Task<BlogPost> CreateAsync(BlogPost blogPost);
    Task<BlogPost> UpdateAsync(BlogPost blogPost);
    Task DeleteAsync(int id);
    Task IncrementViewCountAsync(int id);
}
