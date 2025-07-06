using Academy.Core.Models;

namespace Academy.Application.Services;

public interface IContactService
{
    Task<IEnumerable<ContactMessage>> GetAllMessagesAsync();
    Task<ContactMessage?> GetMessageByIdAsync(int id);
    Task<ContactMessage> SendMessageAsync(ContactMessage message);
    Task<ContactMessage> MarkAsReadAsync(int id);
    Task DeleteMessageAsync(int id);
    Task<IEnumerable<ContactMessage>> GetUnreadMessagesAsync();
} 