using Academy.Core.Models;

namespace Academy.Core.Interfaces;

public interface IContactRepository
{
    Task<IEnumerable<ContactMessage>> GetAllAsync();
    Task<ContactMessage?> GetByIdAsync(int id);
    Task<ContactMessage> AddAsync(ContactMessage message);
    Task<ContactMessage> UpdateAsync(ContactMessage message);
    Task DeleteAsync(int id);
    Task<IEnumerable<ContactMessage>> GetUnreadAsync();
} 