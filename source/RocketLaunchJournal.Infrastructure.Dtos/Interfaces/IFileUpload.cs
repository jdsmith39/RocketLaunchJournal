namespace RocketLaunchJournal.Infrastructure.Dtos.Interfaces
{
    public interface IFileUpload
    {
        string FileKey { get; set; }
        DocumentObjDto Document { get; set; }

        System.IO.Stream FileUpload { get; set; }
    }
}
