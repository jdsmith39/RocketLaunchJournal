using System;

namespace RocketLaunchJournal.Infrastructure
{
    /// <summary>
    /// defines the default paths for a file on disk
    /// </summary>
    public class PathManager
    {
        /// <summary>
        /// Returns the filename for user's Gallery picture
        /// </summary>
        public static string GetUserGalleryFilename(string filename, DateTime dateTime)
        {
            return $"{System.IO.Path.GetFileNameWithoutExtension(filename)}_{dateTime.ToString("yyyyMMddHHmmss")}{System.IO.Path.GetExtension(filename)}";
        }

        /// <summary>
        /// Returns the path for a user's Gallery picture
        /// </summary>
        public static string GetUserGalleryPath(int userId)
        {
            return $"Gallery/{userId}";
        }
    }
}
