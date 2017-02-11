using LiteApi.Attributes;
using System.IO;
using System.Threading.Tasks;

namespace LiteApi.Demo.Controllers
{
    [RestfulLinks]
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
    }
}
