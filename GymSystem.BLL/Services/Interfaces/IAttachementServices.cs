using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IAttachementServices
    {
        Task<string?> UploadingAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default);

        bool Delete(string fileName, string FolderName);

        (Stream stream, string contentType)? GetFile(string fileName, string FolderName);
    }
}
