using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Tests.Controllers
{
    public class FileUploadController : LiteController
    {
        public int Upload(FormFileCollection fileCollection) => fileCollection.FileCount;
    }
}
