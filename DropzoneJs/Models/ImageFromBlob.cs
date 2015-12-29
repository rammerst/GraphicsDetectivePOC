using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphicsDetective.Models
{
    public class ImageFromBlob
    {

        public ImageFromBlob(Uri uri)
        {
            ImageUri = uri;
        }

        public Uri ImageUri { get; set; }


        public bool IsThumb => ImageUri.ToString().EndsWith("_thumbnail.jpg");

    }
}