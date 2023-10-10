namespace WebApp.Models
{
    public static class FileManager
    {
        public static string UploadPhoto(IFormFile file, string CompletePath, string DeleteOldPhoto)
        {
            if (!string.IsNullOrEmpty(DeleteOldPhoto) && System.IO.File.Exists(Path.Combine(CompletePath, DeleteOldPhoto)))
                System.IO.File.Delete(Path.Combine(CompletePath, DeleteOldPhoto));
            return UploadImageToPath(file, CompletePath);
        }

        public static string UploadImageToPath(IFormFile file, string CompletePath)
        {
            string uniquefilename = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filepath = Path.Combine(CompletePath, uniquefilename);
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return uniquefilename;
        }
    }
}
