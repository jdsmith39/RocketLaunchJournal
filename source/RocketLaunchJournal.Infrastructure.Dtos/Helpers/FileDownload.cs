using System.IO;

namespace RocketLaunchJournal.Infrastructure.Dtos.Helpers
{
    /// <summary>
    ///  not a DTO because this should never be passed to the client.
    /// </summary>
    public class FileDownload
    {
        public Stream FileStream { get; set; }

        public byte[] FileBytes { get; set; }

        public string DownloadFilename { get; set; }

        public string FilePath { get; set; }

        public string MimeType { get; set; }
    }
}
