using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Services.Templates.Emails
{
    public class EmailsCreate
    {
        /// <summary>
        /// Generates an email string
        /// </summary>
        /// <param name="mainContent">main content of the email</param>
        /// <param name="footer">footer of the email</param>
        /// <param name="templateEmailPath">embedded resource path to template</param>
        /// <returns>email string</returns>
        public async Task<BaseServiceResponse<string>> GenerateEmail(string baseUrl, string mainContent, string footer, string templateEmailPath = "Team1.Infrastructure.Templates.Emails.base.html")
        {
            var assembly = Assembly.GetAssembly(typeof(EmailsCreate));
            using (Stream stream = assembly.GetManifestResourceStream(templateEmailPath))
            using (StreamReader reader = new StreamReader(stream))
            {
                var email = await reader.ReadToEndAsync();

                email = email.Replace("##url##", baseUrl).Replace("##mainContent##", mainContent).Replace("##footer##", footer);

                return new BaseServiceResponse<string>(email);
            }
        }
    }
}
