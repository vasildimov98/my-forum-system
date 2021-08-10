namespace ForumSystem.Web.Tests.Data
{
    using System.IO;

    using Microsoft.AspNetCore.Http;

    public class FilesTestData
    {
        public static IFormFile GetFile()
        {
            var path = Path.Combine(".", "Data", "TestFiles", "test-image.jpg");

            var stream = File.OpenRead(path);
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
            };

            return file;
        }
    }
}
