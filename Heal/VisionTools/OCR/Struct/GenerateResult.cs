﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

namespace Heal.VisionTools.OCR.Struct
{
    class GenerateResult
    {
        public Image<Bgr, byte> ImageGenerated { get; set; }
        public Rectangle[] BoxChar { get; set; }
        public char[] Char { get; set; }

    }
}