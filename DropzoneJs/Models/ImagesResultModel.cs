﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropzoneJs.Models
{
    public class ImagesResultModel
    {
        public List<String> Images { get; set; }
        public ImagesResultModel()
        {
            Images = new List<string>();
        }
    }
}