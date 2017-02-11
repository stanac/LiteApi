using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace LiteApi.Tests.Fakes
{
    public class FakeFormFile : IFormFile
    {
        public Stream BackingStream { get; set; }

        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }

        public string FileName { get; set; }

        public IHeaderDictionary Headers { get; set; }

        public long Length { get; set; }

        public string Name { get; set; }

        public void CopyTo(Stream target) => BackingStream.CopyTo(target);

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken))
            => BackingStream.CopyToAsync(target);

        public Stream OpenReadStream()
        {
            return BackingStream;
        }
    }
}
