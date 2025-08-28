using Academy.Core.Models;

namespace Academy.Core.Interfaces;

public interface IBlogPostService
{
    Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();
    Task<IEnumerable<BlogPost>> GetPublishedBlogPostsAsync();
    Task<BlogPost?> GetBlogPostByIdAsync(int id);
    Task<BlogPost?> GetBlogPostBySlugAsync(string slug);
    Task<IEnumerable<BlogPost>> GetBlogPostsByCategoryAsync(string category);
    Task<IEnumerable<BlogPost>> GetBlogPostsByTagAsync(string tag);
    Task<IEnumerable<BlogPost>> SearchBlogPostsAsync(string searchTerm);
    Task<IEnumerable<BlogPost>> GetRecentBlogPostsAsync(int count = 5);
    Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync(int count = 5);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<IEnumerable<string>> GetTagsAsync();
    Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);
    Task<BlogPost> UpdateBlogPostAsync(BlogPost blogPost);
    Task DeleteBlogPostAsync(int id);
    Task IncrementViewCountAsync(int id);
}
