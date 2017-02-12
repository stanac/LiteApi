using System.IO;
using System.Text;

namespace LiteApi.Tests.Controllers
{
    public class FileDownloadController: LiteController
    {
        public ILiteActionResult Download1()
        {
            byte[] data = Encoding.UTF8.GetBytes("download1");
            string contentType = "text/plain";
            string fileName = "hello.txt";

            return FileDownload(data, contentType, fileName);
        }

        public ILiteActionResult Download2()
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes("download2");
            using (Stream data = new MemoryStream())
            {
                data.Write(dataBytes, 0, dataBytes.Length);
                string contentType = "text/plain";
                string fileName = "hello.txt";

                return FileDownload(data, contentType, fileName);
            }
        }
    }
}
