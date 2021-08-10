namespace ForumSystem.Web.Tests.Mock
{
    using System.IO;

    using Microsoft.AspNetCore.Hosting;
    using Moq;

    public class IWebHostEnviromentMock
    {
        public static IWebHostEnvironment Create
        {
            get
            {
                var mock = new Mock<IWebHostEnvironment>();

                var path = Path.Combine("..", "..", "..", "Data", "TestFolderForRootPath");

                mock
                    .Setup(env => env.WebRootPath)
                    .Returns(path);

                return mock.Object;
            }
        }
    }
}
