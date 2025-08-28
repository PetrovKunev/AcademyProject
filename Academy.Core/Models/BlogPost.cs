using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Academy.Core.Models;

public class BlogPost
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(500)]
    public string Summary { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? ImageUrl { get; set; }
    
    [MaxLength(100)]
    public string? Author { get; set; }
    
    [MaxLength(50)]
    public string? Category { get; set; }
    
    [JsonIgnore]
    public string TagsJson { get; set; } = "[]";
    
    [NotMapped]
    public List<string> Tags 
    { 
        get => !string.IsNullOrEmpty(TagsJson) ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(TagsJson) ?? new List<string>() : new List<string>();
        set => TagsJson = System.Text.Json.JsonSerializer.Serialize(value);
    }
    
    public bool IsPublished { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? PublishedAt { get; set; }
    
    public int ViewCount { get; set; } = 0;
    
    public string? MetaTitle { get; set; }
    
    [MaxLength(300)]
    public string? MetaDescription { get; set; }
    
    [MaxLength(500)]
    public string? MetaKeywords { get; set; }
}
