using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphicsDetective.Models
{
    public class ImagesResultModel
    {
        public List<ImageFromBlob> Images { get; set; }
        public ImagesResultModel()
        {
            Images = new List<ImageFromBlob>();
        }
    }
}