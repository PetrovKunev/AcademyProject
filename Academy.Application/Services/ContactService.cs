using Academy.Core.Interfaces;
using Academy.Core.Models;

namespace Academy.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IEnumerable<ContactMessage>> GetAllMessagesAsync()
    {
        return await _contactRepository.GetAllAsync();
    }

    public async Task<ContactMessage?> GetMessageByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Message ID must be greater than 0", nameof(id));

        return await _contactRepository.GetByIdAsync(id);
    }

    public async Task<ContactMessage> SendMessageAsync(ContactMessage message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        ValidateMessage(message);
        
        message.CreatedAt = DateTime.UtcNow;
        message.IsRead = false;
        
        return await _contactRepository.AddAsync(message);
    }

    public async Task<ContactMessage> MarkAsReadAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Message ID must be greater than 0", nameof(id));

        var message = await _contactRepository.GetByIdAsync(id);
        if (message == null)
            throw new InvalidOperationException($"Message with ID {id} not found");

        message.IsRead = true;
        return await _contactRepository.UpdateAsync(message);
    }

    public async Task DeleteMessageAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Message ID must be greater than 0", nameof(id));

        var message = await _contactRepository.GetByIdAsync(id);
        if (message == null)
            throw new InvalidOperationException($"Message with ID {id} not found");

        await _contactRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ContactMessage>> GetUnreadMessagesAsync()
    {
        return await _contactRepository.GetUnreadAsync();
    }

    private static void ValidateMessage(ContactMessage message)
    {
        if (string.IsNullOrWhiteSpace(message.Name))
            throw new ArgumentException("Name is required");

        if (string.IsNullOrWhiteSpace(message.Email))
            throw new ArgumentException("Email is required");

        if (!IsValidEmail(message.Email))
            throw new ArgumentException("Invalid email format");

        if (string.IsNullOrWhiteSpace(message.Message))
            throw new ArgumentException("Message content is required");

        if (message.Message.Length > 2000)
            throw new ArgumentException("Message content cannot exceed 2000 characters");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
} 