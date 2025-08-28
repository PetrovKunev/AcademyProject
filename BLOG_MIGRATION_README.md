# Миграция за Блог - Продукционна среда

## Описание
Този документ описва как да приложите миграцията `AddBlogPosts` в продукционната база данни на Академия Логос.

## Файлове
- **SQL скрипт**: `AddBlogPosts.sql` - съдържа SQL командите за създаване на таблицата BlogPosts
- **Миграция**: `Academy.Infrastructure/Migrations/20250828122337_AddBlogPosts.cs`

## SQL скрипт за продукционна среда

```sql
BEGIN TRANSACTION;
CREATE TABLE [BlogPosts] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Summary] nvarchar(500) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Slug] nvarchar(100) NOT NULL,
    [ImageUrl] nvarchar(200) NULL,
    [Author] nvarchar(100) NULL,
    [Category] nvarchar(50) NULL,
    [Tags] nvarchar(max) NOT NULL,
    [IsPublished] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [PublishedAt] datetime2 NULL,
    [ViewCount] int NOT NULL,
    [MetaTitle] nvarchar(200) NULL,
    [MetaDescription] nvarchar(300) NULL,
    [MetaKeywords] nvarchar(500) NULL,
    CONSTRAINT [PK_BlogPosts] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_BlogPosts_Category] ON [BlogPosts] ([Category]);
CREATE INDEX [IX_BlogPosts_CreatedAt] ON [BlogPosts] ([CreatedAt]);
CREATE INDEX [IX_BlogPosts_IsPublished] ON [BlogPosts] ([IsPublished]);
CREATE INDEX [IX_BlogPosts_PublishedAt] ON [BlogPosts] ([PublishedAt]);
CREATE UNIQUE INDEX [IX_BlogPosts_Slug] ON [BlogPosts] ([Slug]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250828122337_AddBlogPosts', N'9.0.6');

COMMIT;
GO
```

## Стъпки за прилагане

### 1. Backup на базата данни
**ВАЖНО**: Преди да приложите миграцията, направете backup на продукционната база данни!

### 2. Прилагане на SQL скрипта
Изпълнете SQL скрипта `AddBlogPosts.sql` в продукционната база данни чрез:
- SQL Server Management Studio (SSMS)
- Azure Data Studio
- Друг SQL клиент

### 3. Проверка
След прилагането проверете:
- Таблицата `BlogPosts` е създадена
- Индексите са създадени
- Записът в `__EFMigrationsHistory` е добавен

### 4. Тестване
Тествайте функционалността на блога:
- `/blog` - основна страница
- `/blog/{slug}` - отделна статия
- `/blog/category/{category}` - по категория
- `/blog/tag/{tag}` - по таг
- `/blog/search` - търсене

## Структура на таблицата

| Колона | Тип | Описание |
|--------|-----|----------|
| Id | int | Primary Key, Identity |
| Title | nvarchar(200) | Заглавие на статията |
| Summary | nvarchar(500) | Кратко описание |
| Content | nvarchar(max) | Пълно съдържание |
| Slug | nvarchar(100) | URL-friendly заглавие (уникално) |
| ImageUrl | nvarchar(200) | URL на изображението |
| Author | nvarchar(100) | Автор |
| Category | nvarchar(50) | Категория |
| Tags | nvarchar(max) | Тагове (JSON формат) |
| IsPublished | bit | Дали е публикувана |
| CreatedAt | datetime2 | Дата на създаване |
| UpdatedAt | datetime2 | Дата на обновяване |
| PublishedAt | datetime2 | Дата на публикуване |
| ViewCount | int | Брой прегледи |
| MetaTitle | nvarchar(200) | SEO заглавие |
| MetaDescription | nvarchar(300) | SEO описание |
| MetaKeywords | nvarchar(500) | SEO ключови думи |

## Индекси
- `IX_BlogPosts_Category` - за бързо търсене по категория
- `IX_BlogPosts_CreatedAt` - за сортиране по дата
- `IX_BlogPosts_IsPublished` - за филтриране на публикувани
- `IX_BlogPosts_PublishedAt` - за сортиране по дата на публикуване
- `IX_BlogPosts_Slug` - уникален индекс за URL

## Rollback (ако е необходимо)
За да отмените миграцията, изпълнете:

```sql
BEGIN TRANSACTION;
DROP TABLE [BlogPosts];
DELETE FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250828122337_AddBlogPosts';
COMMIT;
GO
```

## Забележки
- Миграцията е безопасна и не променя съществуващите данни
- Всички нови колони имат подходящи ограничения за дължина
- Индексите са оптимизирани за често използваните заявки
- Таблицата поддържа пълна SEO функционалност

## Контакти
При проблеми или въпроси, свържете се с екипа за разработка.
