using System.Threading.Tasks;

namespace Services.Email;

/// <summary>
/// Interface for the emailer
/// </summary>
public interface IEmailer
{
    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="To">to email</param>
    /// <param name="Subject">subject of the email</param>
    /// <param name="Body">body of the email</param>
    /// <param name="From">From Address</param>
    /// <param name="ReplyTo">optional replyto email</param>
    /// <param name="CC">array of emails to CC</param>
    /// <param name="Bcc">array of emails to BCC</param>
    /// <param name="bodyType">The email body type</param>
    /// <param name="Attachments">file attachments added to the email</param>
    void SendEmail(string To, string Subject, string Body, string? From = null, string? ReplyTo = null, string[]? CC = null, string[]? Bcc = null, BodyType bodyType = BodyType.HTML, params EmailAttachment[] Attachments);
    
    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="To">to email</param>
    /// <param name="Subject">subject of the email</param>
    /// <param name="Body">body of the email</param>
    /// <param name="From">From Address</param>
    /// <param name="ReplyTo">optional replyto address</param>
    /// <param name="CC">array of emails to CC</param>
    /// <param name="Bcc">array of emails to BCC</param>
    /// <param name="bodyType">The email body type</param>
    /// <param name="Attachments">file attachments added to the email</param>
    void SendEmail(string[] To, string Subject, string Body, string? From = null, string? ReplyTo = null, string[]? CC = null, string[]? Bcc = null, BodyType bodyType = BodyType.HTML, params EmailAttachment[] Attachments);

    /// <summary>
    /// Sends an email async
    /// </summary>
    /// <param name="To">to email</param>
    /// <param name="Subject">subject of the email</param>
    /// <param name="Body">body of the email</param>
    /// <param name="From">From Address</param>
    /// <param name="ReplyTo">optional replyto email</param>
    /// <param name="CC">array of emails to CC</param>
    /// <param name="Bcc">array of emails to BCC</param>
    /// <param name="bodyType">The email body type</param>
    /// <param name="Attachments">file attachments added to the email</param>
    Task SendEmailAsync(string To, string Subject, string Body, string? From = null, string? ReplyTo = null, string[]? CC = null, string[]? Bcc = null, BodyType bodyType = BodyType.HTML, params EmailAttachment[] Attachments);

    /// <summary>
    /// Sends an email async
    /// </summary>
    /// <param name="To">list of to emails</param>
    /// <param name="Subject">subject of the email</param>
    /// <param name="Body">body of the email</param>
    /// <param name="From">From Address</param>
    /// <param name="ReplyTo">optional replyto address</param>
    /// <param name="CC">array of emails to CC</param>
    /// <param name="Bcc">array of emails to BCC</param>
    /// <param name="bodyType">The email body type</param>
    /// <param name="Attachments">file attachments added to the email</param>
    Task SendEmailAsync(string[] To, string Subject, string Body, string? From = null, string? ReplyTo = null, string[]? CC = null, string[]? Bcc = null, BodyType bodyType = BodyType.HTML, params EmailAttachment[] Attachments);
}
