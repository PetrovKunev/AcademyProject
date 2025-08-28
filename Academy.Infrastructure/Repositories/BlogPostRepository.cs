using Academy.Core.Interfaces;
using Academy.Core.Models;
using Academy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Academy.Infrastructure.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly AcademyDbContext _context;

    public BlogPostRepository(AcademyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _context.BlogPosts
            .OrderByDescending(bp => bp.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetPublishedAsync()
    {
        return await _context.BlogPosts
            .Where(bp => bp.IsPublished)
            .OrderByDescending(bp => bp.PublishedAt ?? bp.CreatedAt)
            .ToListAsync();
    }

    public async Task<BlogPost?> GetByIdAsync(int id)
    {
        return await _context.BlogPosts.FindAsync(id);
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug)
    {
        return await _context.BlogPosts
            .FirstOrDefaultAsync(bp => bp.Slug == slug && bp.IsPublished);
    }

    public async Task<IEnumerable<BlogPost>> GetByCategoryAsync(string category)
    {
        return await _context.BlogPosts
            .Where(bp => bp.Category == category && bp.IsPublished)
            .OrderByDescending(bp => bp.PublishedAt ?? bp.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetByTagAsync(string tag)
    {
        var posts = await _context.BlogPosts
            .Where(bp => bp.IsPublished && !string.IsNullOrEmpty(bp.TagsJson))
            .ToListAsync();

        var filteredPosts = new List<BlogPost>();
        foreach (var post in posts)
        {
            try
            {
                var postTags = System.Text.Json.JsonSerializer.Deserialize<List<string>>(post.TagsJson);
                if (postTags != null && postTags.Contains(tag))
                {
                    filteredPosts.Add(post);
                }
            }
            catch
            {
                // Skip invalid JSON
            }
        }

        return filteredPosts.OrderByDescending(bp => bp.PublishedAt ?? bp.CreatedAt);
    }

    public async Task<IEnumerable<BlogPost>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        var posts = await _context.BlogPosts
            .Where(bp => bp.IsPublished)
            .ToListAsync();

        var filteredPosts = posts.Where(bp => 
            bp.Title.ToLower().Contains(term) || 
            bp.Summary.ToLower().Contains(term) || 
            bp.Content.ToLower().Contains(term));

        return filteredPosts.OrderByDescending(bp => bp.PublishedAt ?? bp.CreatedAt);
    }

    public async Task<IEnumerable<BlogPost>> GetRecentAsync(int count = 5)
    {
        return await _context.BlogPosts
            .Where(bp => bp.IsPublished)
            .OrderByDescending(bp => bp.PublishedAt ?? bp.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetPopularAsync(int count = 5)
    {
        return await _context.BlogPosts
            .Where(bp => bp.IsPublished)
            .OrderByDescending(bp => bp.ViewCount)
            .ThenByDescending(bp => bp.PublishedAt ?? bp.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _context.BlogPosts
            .Where(bp => bp.IsPublished && !string.IsNullOrEmpty(bp.Category))
            .Select(bp => bp.Category!)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetTagsAsync()
    {
        var allTags = await _context.BlogPosts
            .Where(bp => bp.IsPublished && !string.IsNullOrEmpty(bp.TagsJson))
            .Select(bp => bp.TagsJson)
            .ToListAsync();

        var tags = new List<string>();
        foreach (var tagsJson in allTags)
        {
            try
            {
                var postTags = System.Text.Json.JsonSerializer.Deserialize<List<string>>(tagsJson);
                if (postTags != null)
                {
                    tags.AddRange(postTags);
                }
            }
            catch
            {
                // Skip invalid JSON
            }
        }

        return tags.Distinct().ToList();
    }

    public async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        _context.BlogPosts.Add(blogPost);
        await _context.SaveChangesAsync();
        return blogPost;
    }

    public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
    {
        blogPost.UpdatedAt = DateTime.UtcNow;
        _context.BlogPosts.Update(blogPost);
        await _context.SaveChangesAsync();
        return blogPost;
    }

    public async Task DeleteAsync(int id)
    {
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost != null)
        {
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
        }
    }

    public async Task IncrementViewCountAsync(int id)
    {
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost != null)
        {
            blogPost.ViewCount++;
            await _context.SaveChangesAsync();
        }
    }
}
