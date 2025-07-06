using System.ComponentModel.DataAnnotations;

namespace Academy.Core.Models;

public class Course
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Level { get; set; } = string.Empty;
    
    [Range(1, 1000)]
    public int Duration { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    
    [StringLength(500)]
    public string? ImageUrl { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
} 