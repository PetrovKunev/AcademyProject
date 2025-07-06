using System.ComponentModel.DataAnnotations;

namespace Academy.Core.Models;

public class ContactMessage
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Subject { get; set; }
    
    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsRead { get; set; } = false;
} 