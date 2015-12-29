using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsDetectiveCommon
{
    public class BlobInformation
    {
        public Uri BlobUri { get; set; }

        public string BlobName => BlobUri.Segments[BlobUri.Segments.Length - 1];

        public string BlobNameWithoutExtension => Path.GetFileNameWithoutExtension(BlobName);

        public int BlobReferenceId { get; set; }
    }
}
