using Academy.Application.Services;
using Academy.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using Academy.Web.Services;

namespace Academy.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;

    public ContactController(IContactService contactService, ILogger<ContactController> logger, IConfiguration configuration, EmailService emailService)
    {
        _contactService = contactService;
        _logger = logger;
        _configuration = configuration;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<ActionResult<ContactMessage>> SendMessage(ContactMessage message)
    {
        try
        {
            var sentMessage = await _contactService.SendMessageAsync(message);
            try
            {
                await _emailService.SendContactEmailAsync(sentMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Възникна грешка при изпращане на имейла: " + ex.Message });
            }
            return CreatedAtAction(nameof(GetMessage), new { id = sentMessage.Id }, sentMessage);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the message", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactMessage>> GetMessage(int id)
    {
        try
        {
            var message = await _contactService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound(new { message = $"Message with ID {id} not found" });
            }

            return Ok(message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the message", error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactMessage>>> GetMessages()
    {
        try
        {
            var messages = await _contactService.GetAllMessagesAsync();
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving messages", error = ex.Message });
        }
    }

    [HttpGet("unread")]
    public async Task<ActionResult<IEnumerable<ContactMessage>>> GetUnreadMessages()
    {
        try
        {
            var messages = await _contactService.GetUnreadMessagesAsync();
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving unread messages", error = ex.Message });
        }
    }

    [HttpPut("{id}/mark-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            var message = await _contactService.MarkAsReadAsync(id);
            return Ok(message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while marking message as read", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        try
        {
            await _contactService.DeleteMessageAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the message", error = ex.Message });
        }
    }
} 