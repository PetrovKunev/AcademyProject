using Academy.Core.Interfaces;
using Academy.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Controllers;

[Route("blog")]
public class BlogController : Controller
{
    private readonly IBlogPostService _blogPostService;

    public BlogController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? category, string? tag, string? search, int page = 1)
    {
        ViewData["Title"] = "Блог - Академия Логос";
        ViewData["Description"] = "Открийте полезни статии за обучение, програмиране, математика и български език от експертите на Академия Логос.";
        
        IEnumerable<BlogPost> posts;
        
        if (!string.IsNullOrEmpty(search))
        {
            posts = await _blogPostService.SearchBlogPostsAsync(search);
            ViewData["SearchTerm"] = search;
        }
        else if (!string.IsNullOrEmpty(category))
        {
            posts = await _blogPostService.GetBlogPostsByCategoryAsync(category);
            ViewData["Category"] = category;
        }
        else if (!string.IsNullOrEmpty(tag))
        {
            posts = await _blogPostService.GetBlogPostsByTagAsync(tag);
            ViewData["Tag"] = tag;
        }
        else
        {
            posts = await _blogPostService.GetPublishedBlogPostsAsync();
        }

        var categories = await _blogPostService.GetCategoriesAsync();
        var tags = await _blogPostService.GetTagsAsync();
        var recentPosts = await _blogPostService.GetRecentBlogPostsAsync(5);
        var popularPosts = await _blogPostService.GetPopularBlogPostsAsync(5);

        ViewData["Categories"] = categories;
        ViewData["Tags"] = tags;
        ViewData["RecentPosts"] = recentPosts;
        ViewData["PopularPosts"] = popularPosts;

        // Simple pagination
        const int pageSize = 6;
        var totalPosts = posts.Count();
        var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
        
        var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        ViewData["TotalPosts"] = totalPosts;

        return View(pagedPosts);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Post(string slug)
    {
        var post = await _blogPostService.GetBlogPostBySlugAsync(slug);
        
        if (post == null)
        {
            return NotFound();
        }

        ViewData["Title"] = post.MetaTitle ?? post.Title;
        ViewData["Description"] = post.MetaDescription ?? post.Summary;
        ViewData["Keywords"] = post.MetaKeywords;
        
        var recentPosts = await _blogPostService.GetRecentBlogPostsAsync(5);
        var popularPosts = await _blogPostService.GetPopularBlogPostsAsync(5);
        var categories = await _blogPostService.GetCategoriesAsync();
        var tags = await _blogPostService.GetTagsAsync();

        ViewData["RecentPosts"] = recentPosts;
        ViewData["PopularPosts"] = popularPosts;
        ViewData["Categories"] = categories;
        ViewData["Tags"] = tags;

        return View(post);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> Category(string category)
    {
        var posts = await _blogPostService.GetBlogPostsByCategoryAsync(category);
        var categoryName = category;

        ViewData["Title"] = $"Категория: {categoryName} - Блог Академия Логос";
        ViewData["Description"] = $"Статии в категория {categoryName} от блога на Академия Логос.";
        ViewData["Category"] = categoryName;

        var categories = await _blogPostService.GetCategoriesAsync();
        var tags = await _blogPostService.GetTagsAsync();
        var recentPosts = await _blogPostService.GetRecentBlogPostsAsync(5);
        var popularPosts = await _blogPostService.GetPopularBlogPostsAsync(5);

        ViewData["Categories"] = categories;
        ViewData["Tags"] = tags;
        ViewData["RecentPosts"] = recentPosts;
        ViewData["PopularPosts"] = popularPosts;

        return View("Index", posts);
    }

    [HttpGet("tag/{tag}")]
    public async Task<IActionResult> Tag(string tag)
    {
        var posts = await _blogPostService.GetBlogPostsByTagAsync(tag);
        var tagName = tag;

        ViewData["Title"] = $"Таг: {tagName} - Блог Академия Логос";
        ViewData["Description"] = $"Статии с таг {tagName} от блога на Академия Логос.";
        ViewData["Tag"] = tagName;

        var categories = await _blogPostService.GetCategoriesAsync();
        var tags = await _blogPostService.GetTagsAsync();
        var recentPosts = await _blogPostService.GetRecentBlogPostsAsync(5);
        var popularPosts = await _blogPostService.GetPopularBlogPostsAsync(5);

        ViewData["Categories"] = categories;
        ViewData["Tags"] = tags;
        ViewData["RecentPosts"] = recentPosts;
        ViewData["PopularPosts"] = popularPosts;

        return View("Index", posts);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return RedirectToAction("Index");
        }

        var posts = await _blogPostService.SearchBlogPostsAsync(q);

        ViewData["Title"] = $"Търсене: {q} - Блог Академия Логос";
        ViewData["Description"] = $"Резултати от търсенето за '{q}' в блога на Академия Логос.";
        ViewData["SearchTerm"] = q;

        var categories = await _blogPostService.GetCategoriesAsync();
        var tags = await _blogPostService.GetTagsAsync();
        var recentPosts = await _blogPostService.GetRecentBlogPostsAsync(5);
        var popularPosts = await _blogPostService.GetPopularBlogPostsAsync(5);

        ViewData["Categories"] = categories;
        ViewData["Tags"] = tags;
        ViewData["RecentPosts"] = recentPosts;
        ViewData["PopularPosts"] = popularPosts;

        return View("Index", posts);
    }
}
