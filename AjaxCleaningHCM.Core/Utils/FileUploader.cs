using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AjaxCleaningHCM.Core.Utils
{
    public static class FileUploader
    {
        public static string UploadFile(IFormFile postedFile)
        {
            try
            {
                if (postedFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    var dbFilePath = "/Uploads/" + fileName;
                    return dbFilePath;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static IFormFile GetFormFileFromPath(string filePath)
        {
            var exactfilepath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\{filePath}";
            string absolutePath = Path.GetFullPath(exactfilepath);
            if (File.Exists(exactfilepath))
            {
                // Read the file into a byte array
                byte[] fileBytes = File.ReadAllBytes(exactfilepath);

                // Create a MemoryStream from the byte array
                var memoryStream = new MemoryStream(fileBytes);

                // Create and return an instance of FormFile
                return new FormFile(memoryStream, 0, fileBytes.Length, null, Path.GetFileName(filePath));
            }
            else
            {
                // Handle the case where the file does not exist
                return null; // or throw an exception
            }
        }
    }

}
