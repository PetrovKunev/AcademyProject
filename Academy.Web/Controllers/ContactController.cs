using Academy.Application.Services;
using Academy.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Academy.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;
    private readonly IConfiguration _configuration;

    public ContactController(IContactService contactService, ILogger<ContactController> logger, IConfiguration configuration)
    {
        _contactService = contactService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<ContactMessage>> SendMessage(ContactMessage message)
    {
        try
        {
            var sentMessage = await _contactService.SendMessageAsync(message);
            // Имейл логика
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            var toEmail = _configuration["EmailSettings:ToEmail"];
            var useSsl = bool.Parse(_configuration["EmailSettings:SmtpUseSsl"]);

            var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = useSsl
            };

            var subject = string.IsNullOrEmpty(message.Subject) ? "Contact Form Submission" : message.Subject;
            var body = $"Име: {message.Name}\nИмейл: {message.Email}\nСъобщение: {message.Message}";
            var mailMessage = new MailMessage(smtpUsername, toEmail, subject, body);
            mailMessage.ReplyToList.Add(new MailAddress(message.Email));
            Console.WriteLine("Преди изпращане на имейл...");
            try
            {
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("Изпратен имейл!");
                _logger.LogInformation("Contact email sent successfully to {ToEmail} from {FromEmail}", toEmail, message.Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при изпращане на имейл: {ex.Message}");
                _logger.LogError(ex, "Failed to send contact email to {ToEmail} from {FromEmail}", toEmail, message.Email);
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