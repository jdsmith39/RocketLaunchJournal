using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Email;

public class MailSettings
{
    /// <summary>
    /// Mail server address
    /// </summary>
    public string Host { get; set; } = string.Empty;
    /// <summary>
    /// Mail server port
    /// 
    /// Defaults to port 25
    /// </summary>
    public int Port { get; set; } = 25;
    
    /// <summary>
    /// Default From address if none is passed in
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// User to authenticate to the mail server with.
    /// </summary>
    public string User { get; set; } = string.Empty;

    /// <summary>
    /// Password used to authenticate the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// DEFAULT is TLS enabled
    /// CAUTION THIS SETTING : Forces the connection to use SSL instead of TLS
    /// </summary>
    public bool UseSsl { get; set; } = false;

    /// <summary>
    /// Flag to indicate if the sending of mail needs to authenticate the user first
    /// 
    /// Default : true
    /// </summary>
    public bool RequiresAuthentication { get; set; } = true;

    /// <summary>
    /// The address to redirect all mail too.
    /// </summary>
    public string RedirectAddress { get; set; } = string.Empty;
}
