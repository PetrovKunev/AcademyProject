using Academy.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Academy.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(AcademyDbContext context)
    {
        context.Database.EnsureCreated();

        // Check if courses already exist
        if (context.Courses.Any())
        {
            return;
        }

        var courses = new Course[]
        {
            new Course
            {
                Title = "JavaScript за начинаещи",
                Description = "Научете основите на JavaScript програмирането. Курсът покрива променливи, функции, обекти, масиви и DOM манипулация.",
                Category = "programming",
                Level = "beginner",
                Duration = 40,
                Price = 299.00m,
                ImageUrl = "/images/courses/javascript-basics.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "React.js - Пълно ръководство",
                Description = "Научете React.js от основите до напреднали техники. Курсът включва хукове, контекст, роутинг и интеграция с API.",
                Category = "programming",
                Level = "intermediate",
                Duration = 60,
                Price = 499.00m,
                ImageUrl = "/images/courses/react-complete.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Node.js и Express.js",
                Description = "Създайте сървърни приложения с Node.js и Express.js. Научете REST API, бази данни, аутентикация и deployment.",
                Category = "programming",
                Level = "intermediate",
                Duration = 50,
                Price = 449.00m,
                ImageUrl = "/images/courses/node-express.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "UX/UI Дизайн основи",
                Description = "Научете принципите на UX/UI дизайна. Курсът покрива потребителски изследвания, wireframing, прототипиране и тестване.",
                Category = "design",
                Level = "beginner",
                Duration = 45,
                Price = 399.00m,
                ImageUrl = "/images/courses/ux-ui-basics.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Figma за дизайнери",
                Description = "Овладейте Figma за създаване на модерни дизайни. Научете компоненти, стилове, прототипи и колаборация.",
                Category = "design",
                Level = "intermediate",
                Duration = 35,
                Price = 349.00m,
                ImageUrl = "/images/courses/figma-design.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Управление на проекти",
                Description = "Научете методики за управление на проекти. Курсът покрива Agile, Scrum, планиране, рискове и комуникация.",
                Category = "business",
                Level = "intermediate",
                Duration = 55,
                Price = 599.00m,
                ImageUrl = "/images/courses/project-management.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Дигитален маркетинг",
                Description = "Научете стратегии за дигитален маркетинг. Курсът включва SEO, социални мрежи, email маркетинг и аналитика.",
                Category = "marketing",
                Level = "beginner",
                Duration = 40,
                Price = 399.00m,
                ImageUrl = "/images/courses/digital-marketing.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Python за data science",
                Description = "Научете Python за анализ на данни. Курсът покрива pandas, numpy, matplotlib и машинно обучение.",
                Category = "programming",
                Level = "advanced",
                Duration = 70,
                Price = 699.00m,
                ImageUrl = "/images/courses/python-data.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Предприемачество и стартиране на бизнес",
                Description = "Научете как да стартирате и развивате собствен бизнес. Курсът покрива бизнес планове, финанси и стратегии.",
                Category = "business",
                Level = "beginner",
                Duration = 50,
                Price = 549.00m,
                ImageUrl = "/images/courses/entrepreneurship.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Courses.AddRange(courses);
        context.SaveChanges();
    }
} 