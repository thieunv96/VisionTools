using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading;

namespace Heal.VisionTools.OCR.Utils
{
    class DrawCharacter
    {
        public Bitmap DrawText(string Content, Font TextFont,Color Backgound, Color Foreground)
        {
            Bitmap outImg = null;
            SizeF textSize = new SizeF(1, 1);
            using (Image img = new Bitmap(1, 1))
            using (Graphics temp = Graphics.FromImage(img))
            {
                textSize = temp.MeasureString(Content, TextFont);
            }
            outImg = new Bitmap((int)textSize.Width, (int)textSize.Height);
            using (Graphics g = Graphics.FromImage(outImg))
            {
                g.Clear(Backgound);
                Brush textBrush = new SolidBrush(Foreground);
                g.DrawString(Content, TextFont, textBrush, 0, 0);
            }
            return outImg;
        }
        public DrawResult DrawText(string Content, Font TextFont,  Color TextColor)
        {
            DrawResult result = new DrawResult();
            using (Bitmap bmMask = DrawText(Content, TextFont, Color.White, Color.Black))
            {
                result.Mask = new Image<Bgr, byte>(bmMask);
            }
            using (Bitmap image = DrawText(Content, TextFont, Color.Black, TextColor))
            {
                result.Image = new Image<Bgr, byte>(image);
            }
            return result;
        }
    }
    class DrawResult
    {
        public Image<Bgr, byte> Image { get; set; }
        public  Image<Bgr, byte> Mask { get; set; }
        public void Dispose()
        {
            if(this.Image != null)
            {
                this.Image.Dispose();
                this.Image = null;
            }
            if (this.Mask != null)
            {
                this.Mask.Dispose();
                this.Mask = null;
            }
        }
    }
}
