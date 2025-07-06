using Academy.Core.Interfaces;
using Academy.Core.Models;
using Academy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Academy.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AcademyDbContext _context;

    public ContactRepository(AcademyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContactMessage>> GetAllAsync()
    {
        return await _context.ContactMessages
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<ContactMessage?> GetByIdAsync(int id)
    {
        return await _context.ContactMessages.FindAsync(id);
    }

    public async Task<ContactMessage> AddAsync(ContactMessage message)
    {
        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<ContactMessage> UpdateAsync(ContactMessage message)
    {
        _context.ContactMessages.Update(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task DeleteAsync(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message != null)
        {
            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ContactMessage>> GetUnreadAsync()
    {
        return await _context.ContactMessages
            .Where(c => !c.IsRead)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
} 