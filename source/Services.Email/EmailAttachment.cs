using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Email
{
    /// <summary>
    /// Container for files that will be attached to an email.
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// Name of the file when attached.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Binary data for the attachment.
        /// </summary>
        public Stream Data { get; set; }
    }
}
