using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Common.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        public readonly List<string> _allowedExtensions = new List<string>{".jpg", ".jpeg", ".png" };
        public const int _maxFileSize = 2 * 1024 * 1024; // 2 MB
        public async Task<string?> UploadAsync(IFormFile file, string folderName)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!_allowedExtensions.Contains(extension))
            {
                return null; // Invalid file type
            }
            if(file.Length > _maxFileSize)
            {
                return null; // File size exceeds limit
            }
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files", folderName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return fileName;
        }
        public bool Delete(string filePath)
        {
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false; // File does not exist
        }

    }
}
