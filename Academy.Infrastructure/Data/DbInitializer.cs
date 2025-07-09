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
                Title = "Математика - 5 клас",
                Description = "Целогодишен курс по математика за 5 клас. Подготовка за отличен успех и стабилни знания.",
                Category = "Математика",
                Level = "",
                Duration = 100,
                Price = 350.00m,
                ImageUrl = "/images/courses/Math_5.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Математика - 6 клас",
                Description = "Целогодишен курс по математика за 6 клас. Задълбочено изучаване и упражнения.",
                Category = "Математика",
                Level = "",
                Duration = 100,
                Price = 370.00m,
                ImageUrl = "/images/courses/Math_6.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Математика - 7 клас /Подготовка за НВО/",
                Description = "Интензивна подготовка по математика за НВО след 7 клас. Решаване на тестове и задачи.",
                Category = "Математика",
                Level = "",
                Duration = 120,
                Price = 420.00m,
                ImageUrl = "/images/courses/Math_7.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "БЕЛ - 5 клас",
                Description = "Целогодишен курс по български език и литература за 5 клас. Развитие на езикови и литературни умения.",
                Category = "БЕЛ",
                Level = "",
                Duration = 100,
                Price = 350.00m,
                ImageUrl = "/images/courses/BEL_5.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "БЕЛ - 6 клас",
                Description = "Целогодишен курс по български език и литература за 6 клас. Подготовка за отличен успех.",
                Category = "БЕЛ",
                Level = "",
                Duration = 100,
                Price = 370.00m,
                ImageUrl = "/images/courses/BEL_6.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "БЕЛ - 7 клас /Подготовка за НВО/",
                Description = "Интензивна подготовка по БЕЛ за НВО след 7 клас. Анализ на текстове и упражнения.",
                Category = "БЕЛ",
                Level = "",
                Duration = 120,
                Price = 420.00m,
                ImageUrl = "/images/courses/BEL_7.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Courses.AddRange(courses);
        context.SaveChanges();
    }
} 