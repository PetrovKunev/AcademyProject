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

