using LiteApi.Attributes;
using System.IO;
using System.Threading.Tasks;

namespace LiteApi.Demo.Controllers
{
    [RestfulLinks]
    public class FileController: LiteController
    {
        [HttpPost]
        public async Task<int> Upload()
        {
            using (Stream memStream = new MemoryStream())
            {
                Microsoft.AspNetCore.Http.IFormCollection f = await HttpContext.Request.ReadFormAsync();
                Microsoft.AspNetCore.Http.IFormFile f0 = f.Files[0];
                using (Stream readS = f0.OpenReadStream())
                using (Stream writeS = new FileStream("d:/" + f0.FileName, FileMode.Create, FileAccess.Write))
                {
                    readS.CopyTo(writeS);
                }
            }
            return 1;
        }
    }
}
