using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Heal.VisionTools.OCR.Utils
{
    class DrawCharacter
    {
        public DrawResult DrawText(string Content, Font TextFont,Color Backgound, Color Foreground)
        {
            DrawResult result = new DrawResult();
            SizeF textSize = new SizeF(1, 1);
            using (Image img = new Bitmap(1, 1))
            using (Graphics temp = Graphics.FromImage(img))
            {
                textSize = temp.MeasureString(Content, TextFont);
            }
            Bitmap outImg = new Bitmap((int)textSize.Width, (int)textSize.Height);
            using (Graphics drawing = Graphics.FromImage(outImg))
            {
                drawing.Clear(Backgound);
                Brush textBrush = new SolidBrush(Foreground);
                drawing.DrawString(Content, TextFont, textBrush, 0, 0);
            }
            return result;
        }
    }
    class DrawResult
    {
        public Image<Bgr, byte> Image { get; set; }
        public  Image<Bgr, byte> Mask { get; set; }
    }
}
