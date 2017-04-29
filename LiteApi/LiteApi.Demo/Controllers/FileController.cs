using LiteApi.Attributes;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LiteApi.Demo.Controllers
{
    [Restful]
    public class FileController: LiteController
    {
        [HttpPost]
        public async Task<long> Upload(FormFileCollection fileCollection)
        {
            long bytesUploaded = 0;
            foreach (var file in fileCollection.Files)
            {
                using (Stream fileStream = new FileStream("d:\\" + file.FileName, FileMode.Create))
                {
                    bytesUploaded += file.Length;
                    await file.CopyToAsync(fileStream);
                }
            }

            return bytesUploaded;
        }

        [HttpGet]
        public ILiteActionResult Download()
        {
            byte[] data = Encoding.UTF8.GetBytes("hello from LiteApi"); // can be Stream or byte[]
            string contentType = "text/plain";
            string fileName = "hello.txt";

            return FileDownload(data, contentType, fileName);
        }
    }
}
