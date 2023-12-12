using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using RocketLaunchJournal.Infrastructure.Dtos.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// File Download and delete file result
/// returns the file and then deletes it after download is complete.
/// </summary>
public class FileDownloadAndDeleteFileResult : FileResult
{
    private string _filePath;

    private Stream _fileStream;

    /// <summary>
    /// File result and deletes the file after download
    /// </summary>
    /// <param name="filePath">file full path</param>
    /// <param name="contentType">content type/mime type</param>
    /// <param name="downloadFileName">name of the file when user prompted for download</param>
    public FileDownloadAndDeleteFileResult(string filePath, string contentType, string downloadFileName) : base(contentType)
    {
        _fileStream = new FileStream(filePath, FileMode.Open);
        _filePath = filePath;
        FileDownloadName = downloadFileName;
    }

    /// <summary>
    /// File result and deletes the file after download
    /// </summary>
    /// <param name="fileStream">file stream</param>
    /// <param name="filePathToDelete">path of the file to delete</param>
    /// <param name="contentType">content type/mime type</param>
    /// <param name="downloadFileName">name of the file when user prompted for download</param>
    public FileDownloadAndDeleteFileResult(Stream fileStream, string filePathToDelete, string contentType, string downloadFileName) : base(contentType)
    {
        _fileStream = fileStream;
        _filePath = filePathToDelete;
        FileDownloadName = downloadFileName;
    }

    /// <summary>
    /// File result and deletes the file after download
    /// </summary>
    /// <param name="fileDownload">contains details about the file download</param>
    public FileDownloadAndDeleteFileResult(FileDownload fileDownload) : base(fileDownload.MimeType)
    {
        _fileStream = fileDownload.FileStream ?? new FileStream(fileDownload.FilePath, FileMode.Open);
        _filePath = fileDownload.FilePath;
        FileDownloadName = fileDownload.DownloadFilename;
    }

    /// <summary>
    /// sets up the response
    /// </summary>
    public async override Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.ContentType = ContentType;
        var contentDisposition = new ContentDispositionHeaderValue("attachment");
        contentDisposition.SetHttpFileName(FileDownloadName);
        response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
        response.StatusCode = StatusCodes.Status200OK;
        using (_fileStream)
        {
            await StreamCopyOperation.CopyToAsync(_fileStream, response.Body, null, context.HttpContext.RequestAborted);
        }
        File.Delete(_filePath);
    }
}
