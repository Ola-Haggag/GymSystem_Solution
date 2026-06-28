using GymSystem.BLL.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace GymSystem.BLL.Services.Classes
{
    public class AttachementServices : IAttachementServices
    {
        public AttachementServices(ILogger<AttachementServices> logger, IWebHostEnvironment environment)
        {
            this.logger = logger;
            this.environment = environment;
        }
        private readonly long maxFileSize = 5 * 1024 * 1024;// 5mb
        private readonly string[] allowedExtentions = {".jpg",".jpeg",".png" };
        private readonly ILogger<AttachementServices> logger;
        private readonly IWebHostEnvironment environment;

        public bool Delete(string fileName, string FolderName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrEmpty(FolderName)) return false;

            try
            {
                var FullPath = Path.Combine(environment.ContentRootPath, FolderName, fileName);

                if(!File.Exists(FullPath)) return false;

                File.Delete(FullPath);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Failed to delete  Attachement");
                return false;
            }
        }

        public (Stream stream, string contentType)? GetFile(string fileName, string FolderName)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> UploadingAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead) return null;
            if (fileStream.Length == 0) return null;
            if(fileStream.Length > maxFileSize)
            {
                logger.LogWarning("Rejected File too large");
                return null;
            }
            var Extention = Path.GetExtension(fileName);
            if(string.IsNullOrEmpty(Extention) || !allowedExtentions.Contains(Extention))
            {
                logger.LogWarning(" rejected wrong extention file");
                return null;
            }

            var UploadedFolder = Path.Combine(environment.ContentRootPath, folderName);

            Directory.CreateDirectory(UploadedFolder);
            var storedFileName = $"{Guid.NewGuid()}{Extention}";
            var filePath = Path.Combine(UploadedFolder, storedFileName);

            try
            {
                await using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                 await fileStream.CopyToAsync(fs);
                return storedFileName;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed to upload file");
                return null;
            }
        }
    }
}
