using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Services.Email;

/// <summary>
/// Emails using the standard .NET libraries.
/// </summary>
public class Emailer : IEmailer
{
    private MailSettings _settings;

    /// <summary>
    /// Initializes the emailer using the current executing directory
    /// </summary>
    /// <param name="applicationPath"></param>
    public Emailer()
        : this(Directory.GetCurrentDirectory())
    {
    }

    /// <summary>
    /// Initializes with the IOptions Mail Settings
    /// </summary>
    /// <param name="mailSettings">IOptions MailSettings</param>
    public Emailer(IOptionsSnapshot<MailSettings> mailSettings)
    {
        _settings = mailSettings.Value;
    }

    /// <summary>
    /// Initializes the emailer with the appsettings.json path
    /// </summary>
    /// <param name="appSettingsPath">path for the appsettings.json file</param>
    public Emailer(string appSettingsPath)
    {
        _settings = new MailSettings();

        var config = new ConfigurationBuilder()
            .SetBasePath(appSettingsPath)
            .AddJsonFile(@"appsettings.json", false, true)
            .Build();

        //Get new settings first and if that is empty then load from the config files.
        _settings = config.GetSection("MailSettings").Get<MailSettings>();

        //if the host is still not defined throw an exception
        if (String.IsNullOrWhiteSpace(_settings.Host))
        {
            throw new Exception("Unable to read configuration.  Please add a configuration or use the loaded constructor.");
        }
    }

    /// <summary>
    /// Initializes the emailer with the mail settings object
    /// </summary>
    /// <param name="settings"></param>
    public Emailer(MailSettings settings)
    {
        _settings = settings;
    }

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
    public void SendEmail(string To, string Subject, string Body, string? From = null, string? ReplyTo = null, string[]? CC = null, string[]? Bcc = null, BodyType bodyType = BodyType.HTML, params EmailAttachment[] Attachments)
    {
        var task = Task.Run(async () => await SendEmailAsync(new string[] { To }, Subject, Body, From, ReplyTo, CC, Bcc, bodyType, Attachments));
        task.Wait();
    }

    /// <summary>
    /// Sends an email
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
    public void SendEmail(string[] To, string Subject, string Body, string? From = null, string? ReplyTo = null, string[]? CC = null, string[]? Bcc = null, BodyType bodyType = BodyType.HTML, params EmailAttachment[] Attachments)
    {
        var task = Task.Run(async () => await SendEmailAsync(To, Subject, Body, From, ReplyTo, CC, Bcc, bodyType, Attachments));
        task.Wait();
    }

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
    public Task SendEmailAsync(string To, string Subject, string Body, string? From = null, string? ReplyTo = null, string[]? CC = null, string[]? Bcc = null, BodyType bodyType = BodyType.HTML, params EmailAttachment[] Attachments)
    {
        return Task.Run(() => SendEmail(new string[] { To }, Subject, Body, From, ReplyTo, CC, Bcc, bodyType, Attachments));
    }

    /// <summary>
    /// Sends an email async
    /// </summary>
    /// <param name="To">to email</param>
    /// <param name="Subject">subject of the email</param>
    /// <param name="Body">body of the email</param>
    /// <param name="From">From Address</param>
    /// <param name="ReplyTo">optional replyto address</param>
    /// <param name="CC">array of emails to CC</param>
    /// <param name="Bcc">array of emails to BCC</param>
    /// <param name="Attachments">file attachments added to the email</param>
    public async Task SendEmailAsync(string[] To, string Subject, string Body, string? From = null, string? ReplyTo = null, string[]? CC = null, string[]? Bcc = null, BodyType bodyType = BodyType.HTML, params EmailAttachment[] Attachments)
    {
        // Validate parameters
        if (To.Length <= 0 || String.IsNullOrWhiteSpace(To[0]))
        {
            throw new ArgumentException("no to addresses provided");
        }

        if (String.IsNullOrWhiteSpace(From) && String.IsNullOrWhiteSpace(_settings.From))
        {
            throw new ArgumentException("no from address provided");
        }

        if (String.IsNullOrWhiteSpace(Subject))
        {
            throw new ArgumentException("no subject provided");
        }

        if (String.IsNullOrWhiteSpace(Body))
        {
            throw new ArgumentException("no message provided");
        }

        if (String.IsNullOrWhiteSpace(From) && !String.IsNullOrWhiteSpace(_settings.From))
            From = _settings.From;


        //Redirect emails 
        if (!String.IsNullOrWhiteSpace(_settings.RedirectAddress))
        {
            var redirectedFrom = To;
            To = _settings.RedirectAddress.Split(';');
            Body = $"<p>[Redirected from {string.Join(",", redirectedFrom)}]</p>{Body}";
        }

        var m = new MimeMessage();
        m.From.Add(MailboxAddress.Parse(From));

        if (!string.IsNullOrWhiteSpace(ReplyTo))
        {
            m.ReplyTo.Add(new MailboxAddress(ReplyTo, ReplyTo));
        }

        foreach (string item in To)
        {
            if (!string.IsNullOrEmpty(item)) { m.To.Add(MailboxAddress.Parse(item)); }
        }

        if (CC != null && CC.Length > 0)
        {
            foreach (string item in CC)
                if (!string.IsNullOrEmpty(item)) { m.Cc.Add(MailboxAddress.Parse(item)); }
        }

        if (Bcc != null && Bcc.Length > 0)
        {
            foreach (string item in Bcc)
                if (!string.IsNullOrEmpty(item)) { m.Bcc.Add(MailboxAddress.Parse(item)); }
        }

        m.Subject = Subject;
        m.Importance = MessageImportance.High;

        BodyBuilder bodyBuilder = new BodyBuilder();
        if (bodyType == BodyType.HTML)
        {
            bodyBuilder.HtmlBody = Body;
        }
        else
        {
            bodyBuilder.TextBody = Body;
        }

        if (Attachments != null && Attachments.Length > 0)
        {
            foreach (var item in Attachments)
            {
                bodyBuilder.Attachments.Add(item.FileName, item.Data);
            }
        }

        m.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(
            _settings.Host,
            _settings.Port,
            _settings.UseSsl).ConfigureAwait(false);

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            // Note: only needed if the SMTP server requires authentication
            if (_settings.RequiresAuthentication)
            {
                await client.AuthenticateAsync(
                    _settings.User,
                    _settings.Password).ConfigureAwait(false);
            }

            await client.SendAsync(m).ConfigureAwait(false);
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}
