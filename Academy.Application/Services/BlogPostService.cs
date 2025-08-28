using Academy.Core.Interfaces;
using Academy.Core.Models;

namespace Academy.Application.Services;

public class BlogPostService : IBlogPostService
{
    private readonly IBlogPostRepository _blogPostRepository;

    public BlogPostService(IBlogPostRepository blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
    }

    public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
    {
        return await _blogPostRepository.GetAllAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetPublishedBlogPostsAsync()
    {
        return await _blogPostRepository.GetPublishedAsync();
    }

    public async Task<BlogPost?> GetBlogPostByIdAsync(int id)
    {
        return await _blogPostRepository.GetByIdAsync(id);
    }

    public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
    {
        var blogPost = await _blogPostRepository.GetBySlugAsync(slug);
        if (blogPost != null)
        {
            await _blogPostRepository.IncrementViewCountAsync(blogPost.Id);
        }
        return blogPost;
    }

    public async Task<IEnumerable<BlogPost>> GetBlogPostsByCategoryAsync(string category)
    {
        return await _blogPostRepository.GetByCategoryAsync(category);
    }

    public async Task<IEnumerable<BlogPost>> GetBlogPostsByTagAsync(string tag)
    {
        return await _blogPostRepository.GetByTagAsync(tag);
    }

    public async Task<IEnumerable<BlogPost>> SearchBlogPostsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Enumerable.Empty<BlogPost>();
            
        return await _blogPostRepository.SearchAsync(searchTerm);
    }

    public async Task<IEnumerable<BlogPost>> GetRecentBlogPostsAsync(int count = 5)
    {
        return await _blogPostRepository.GetRecentAsync(count);
    }

    public async Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync(int count = 5)
    {
        return await _blogPostRepository.GetPopularAsync(count);
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _blogPostRepository.GetCategoriesAsync();
    }

    public async Task<IEnumerable<string>> GetTagsAsync()
    {
        return await _blogPostRepository.GetTagsAsync();
    }

    public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost)
    {
        // Generate slug if not provided
        if (string.IsNullOrEmpty(blogPost.Slug))
        {
            blogPost.Slug = GenerateSlug(blogPost.Title);
        }

        // Set creation date
        blogPost.CreatedAt = DateTime.UtcNow;
        
        // Set published date if publishing
        if (blogPost.IsPublished && !blogPost.PublishedAt.HasValue)
        {
            blogPost.PublishedAt = DateTime.UtcNow;
        }

        return await _blogPostRepository.CreateAsync(blogPost);
    }

    public async Task<BlogPost> UpdateBlogPostAsync(BlogPost blogPost)
    {
        var existingPost = await _blogPostRepository.GetByIdAsync(blogPost.Id);
        if (existingPost == null)
            throw new ArgumentException("Blog post not found");

        // Update published date if status changed to published
        if (blogPost.IsPublished && !existingPost.IsPublished)
        {
            blogPost.PublishedAt = DateTime.UtcNow;
        }

        return await _blogPostRepository.UpdateAsync(blogPost);
    }

    public async Task DeleteBlogPostAsync(int id)
    {
        await _blogPostRepository.DeleteAsync(id);
    }

    public async Task IncrementViewCountAsync(int id)
    {
        await _blogPostRepository.IncrementViewCountAsync(id);
    }

    private string GenerateSlug(string title)
    {
        // Simple slug generation - convert to lowercase, replace spaces with hyphens, remove special chars
        var slug = title.ToLower()
            .Replace(" ", "-")
            .Replace("а", "a").Replace("б", "b").Replace("в", "v").Replace("г", "g")
            .Replace("д", "d").Replace("е", "e").Replace("ж", "zh").Replace("з", "z")
            .Replace("и", "i").Replace("й", "y").Replace("к", "k").Replace("л", "l")
            .Replace("м", "m").Replace("н", "n").Replace("о", "o").Replace("п", "p")
            .Replace("р", "r").Replace("с", "s").Replace("т", "t").Replace("у", "u")
            .Replace("ф", "f").Replace("х", "h").Replace("ц", "ts").Replace("ч", "ch")
            .Replace("ш", "sh").Replace("щ", "sht").Replace("ъ", "a").Replace("ь", "y")
            .Replace("ю", "yu").Replace("я", "ya");

        // Remove special characters
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");
        
        // Remove multiple hyphens
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
        
        // Remove leading and trailing hyphens
        slug = slug.Trim('-');

        return slug;
    }
}
