Introduction
Spud Software Emailer Sending Emails using the standard .NET libraries and mailkit

Getting Started

By default the SpudEmailer uses the appsettings.json file and will map the "MailSettings" object. 

/* Sample /
"MailSettings": { "Host": "mail.spudsoftware.com", "Port": "587", "From": "system@pacmanageronline.com", 
"User": "system@pacmanageronline.com", "Password": "JS9v#2XkxZ32Heq", 
"UseSsl": false, 
"RequiresAuthentication": true, "RedirectAddress": "name@Spudsoftware.com" }
/* End Sample /

You can also fill in a "MailSettings" object manually and use that in your SpudEmailer constructor

Usage:

.Net 4.6.1+ Dependency Injection with IEmailer to SpudEmailer Unity: 
container.RegisterType<IEmailer, SpudEmailer>(new InjectionFactory(c => new SpudEmailer(HttpRuntime.AppDomainAppPath)) );

aspnet Core startup IOptions<MailSettings> DI (located in ConfigureServices function) preferred: 
services.Configure<Spud.Services.Email.MailSettings>(Configuration.GetSection("MailSettings")); 
services.AddOptions(); 
services.AddTransient<Spud.Services.Email.IEmailer, Spud.Services.Email.SpudEmailer>();

aspnet Core startup Injecting MailSettings directly (located in ConfigureServices function): 
services.AddTransient<Spud.Services.Email.IEmailer, Spud.Services.Email.SpudEmailer>((s) => 
{
return new Spud.Services.Email.SpudEmailer(Configuration.GetSection("MailSettings").Get<Spud.Services.Email.MailSettings>());
});

.Net core console app, the above aspnet Core methods can also be used within .Net core console app. 
var emailer = new SpudEmailer(); // assumes appsettings.json is in the same folder as the *.exe file

To address is a single string 
SpudEmailer.SendEmail(string To, string Subject, string Body, string From = null, string ReplyTo = null, string[] CC = null, string[] Bcc = null, params EmailAttachment[] Attachments); 

To address is an array of strings 
SpudEmailer.SendEmail(string[] To,string Subject, string Body, string From = null, string ReplyTo = null, string[] CC = null, string[] Bcc = null, params EmailAttachment[] Attachments);

Also contains the preferred Async versions: await SpudEmailer.SendEmailAsync(...);