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
    public async Task<IActionResult> Category(string category, int page = 1)
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
        
        // Simple pagination
        const int pageSize = 6;
        var totalPosts = posts.Count();
        var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
        
        var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        ViewData["TotalPosts"] = totalPosts;

        return View("Index", pagedPosts);
    }

    [HttpGet("tag/{tag}")]
    public async Task<IActionResult> Tag(string tag, int page = 1)
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
        
        // Simple pagination
        const int pageSize = 6;
        var totalPosts = posts.Count();
        var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
        
        var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        ViewData["TotalPosts"] = totalPosts;

        return View("Index", pagedPosts);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string q, int page = 1)
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
        
        // Simple pagination
        const int pageSize = 6;
        var totalPosts = posts.Count();
        var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
        
        var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        ViewData["TotalPosts"] = totalPosts;

        return View("Index", pagedPosts);
    }

    // ===== АДМИНИСТРАТИВНИ МЕТОДИ =====
    
    [HttpGet("admin")]
    public IActionResult Admin()
    {
        // Проверка дали потребителят е влязъл
        if (!IsAdminAuthenticated())
        {
            return RedirectToAction("AdminLogin");
        }
        
        ViewData["Title"] = "Администрация - Блог Академия Логос";
        ViewData["Description"] = "Управление на блог статиите.";
        
        return View();
    }

    [HttpGet("admin/login")]
    public IActionResult AdminLogin()
    {
        // Ако вече е влязъл, пренасочваме към администрацията
        if (IsAdminAuthenticated())
        {
            return RedirectToAction("Admin");
        }
        
        ViewData["Title"] = "Вход за администратори - Академия Логос";
        ViewData["Description"] = "Влезте в административния панел.";
        
        return View();
    }

    [HttpPost("admin/login")]
    public IActionResult AdminLogin(string username, string password)
    {
        // Проверка на credentials
        if (username == "admin" && password == "academy2024")
        {
            // Задаване на сесия
            HttpContext.Session.SetString("AdminAuthenticated", "true");
            HttpContext.Session.SetString("AdminUsername", username);
            
            return RedirectToAction("Admin");
        }
        
        // Грешен вход
        ModelState.AddModelError("", "Грешно име или парола!");
        ViewData["Title"] = "Вход за администратори - Академия Логос";
        ViewData["Description"] = "Влезте в административния панел.";
        
        return View();
    }

    [HttpPost("admin/logout")]
    public IActionResult AdminLogout()
    {
        // Изчистване на сесията
        HttpContext.Session.Clear();
        
        return RedirectToAction("AdminLogin");
    }

    private bool IsAdminAuthenticated()
    {
        return HttpContext.Session.GetString("AdminAuthenticated") == "true";
    }

    [HttpGet("admin/create")]
    public IActionResult AdminCreate()
    {
        // Проверка дали потребителят е влязъл
        if (!IsAdminAuthenticated())
        {
            return RedirectToAction("AdminLogin");
        }
        
        ViewData["Title"] = "Създаване на статия - Администрация";
        ViewData["Description"] = "Създайте нова статия за блога.";
        
        return View();
    }

    [HttpPost("admin/create")]
    public async Task<IActionResult> AdminCreate(BlogPost blogPost)
    {
        // Проверка дали потребителят е влязъл
        if (!IsAdminAuthenticated())
        {
            return RedirectToAction("AdminLogin");
        }
        
        if (ModelState.IsValid)
        {
            try
            {
                // Генериране на slug ако не е предоставен
                if (string.IsNullOrEmpty(blogPost.Slug))
                {
                    blogPost.Slug = GenerateSlug(blogPost.Title);
                }

                // Задаване на датата на създаване
                blogPost.CreatedAt = DateTime.UtcNow;
                
                // Задаване на датата на публикуване ако е публикувана
                if (blogPost.IsPublished && !blogPost.PublishedAt.HasValue)
                {
                    blogPost.PublishedAt = DateTime.UtcNow;
                }

                // Инициализиране на броя прегледи
                blogPost.ViewCount = 0;

                var createdPost = await _blogPostService.CreateBlogPostAsync(blogPost);
                
                TempData["SuccessMessage"] = "Статията е създадена успешно!";
                return RedirectToAction("Post", new { slug = createdPost.Slug });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Грешка при създаването на статията: " + ex.Message);
            }
        }
        
        ViewData["Title"] = "Създаване на статия - Администрация";
        ViewData["Description"] = "Създайте нова статия за блога.";
        
        return View(blogPost);
    }

    [HttpGet("admin/edit/{id}")]
    public async Task<IActionResult> AdminEdit(int id)
    {
        // Проверка дали потребителят е влязъл
        if (!IsAdminAuthenticated())
        {
            return RedirectToAction("AdminLogin");
        }
        
        var post = await _blogPostService.GetBlogPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        
        ViewData["Title"] = "Редактиране на статия - Администрация";
        ViewData["Description"] = "Редактирайте съществуваща статия.";
        
        return View(post);
    }

    [HttpPost("admin/edit/{id}")]
    public async Task<IActionResult> AdminEdit(int id, BlogPost blogPost)
    {
        // Проверка дали потребителят е влязъл
        if (!IsAdminAuthenticated())
        {
            return RedirectToAction("AdminLogin");
        }
        
        if (ModelState.IsValid)
        {
            try
            {
                var existingPost = await _blogPostService.GetBlogPostByIdAsync(id);
                if (existingPost == null)
                {
                    return NotFound();
                }

                // Обновяване на полетата
                existingPost.Title = blogPost.Title;
                existingPost.Summary = blogPost.Summary;
                existingPost.Content = blogPost.Content;
                existingPost.Category = blogPost.Category;
                existingPost.Author = blogPost.Author;
                existingPost.ImageUrl = blogPost.ImageUrl;
                existingPost.IsPublished = blogPost.IsPublished;
                existingPost.MetaTitle = blogPost.MetaTitle;
                existingPost.MetaDescription = blogPost.MetaDescription;
                existingPost.MetaKeywords = blogPost.MetaKeywords;
                existingPost.UpdatedAt = DateTime.UtcNow;

                // Обновяване на датата на публикуване
                if (blogPost.IsPublished && !existingPost.PublishedAt.HasValue)
                {
                    existingPost.PublishedAt = DateTime.UtcNow;
                }
                else if (!blogPost.IsPublished)
                {
                    existingPost.PublishedAt = null;
                }

                await _blogPostService.UpdateBlogPostAsync(existingPost);
                
                TempData["SuccessMessage"] = "Статията е обновена успешно!";
                return RedirectToAction("Post", new { slug = existingPost.Slug });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Грешка при обновяването на статията: " + ex.Message);
            }
        }
        
        ViewData["Title"] = "Редактиране на статия - Администрация";
        ViewData["Description"] = "Редактирайте съществуваща статия.";
        
        return View(blogPost);
    }

    [HttpGet("admin/list")]
    public async Task<IActionResult> AdminList()
    {
        // Проверка дали потребителят е влязъл
        if (!IsAdminAuthenticated())
        {
            return RedirectToAction("AdminLogin");
        }
        
        var posts = await _blogPostService.GetAllBlogPostsAsync();
        
        ViewData["Title"] = "Списък на статиите - Администрация";
        ViewData["Description"] = "Управление на всички блог статии.";
        
        return View(posts);
    }

    private string GenerateSlug(string title)
    {
        // Просто генериране на slug - конвертиране в малки букви, замяна на интервали с тирета, премахване на специални символи
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

        // Премахване на специални символи
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");
        
        // Премахване на множествени тирета
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
        
        // Премахване на водещи и крайни тирета
        slug = slug.Trim('-');

        return slug;
    }
}
