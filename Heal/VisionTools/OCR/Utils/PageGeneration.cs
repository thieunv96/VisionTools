using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;

namespace Heal.VisionTools.OCR.Utils
{
    class PageGeneration
    {
        public Image<Bgr, byte> GetPageImage(Struct.GenConfig Config)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Config.ImageSetting.ImageSize);

            return null;
        }
    }
}
