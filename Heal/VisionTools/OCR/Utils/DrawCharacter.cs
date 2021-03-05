using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Threading;

namespace Heal.VisionTools.OCR.Utils
{
    class DrawCharacter
    {
        public Rectangle GetExtremeBouding(Image<Gray, byte> image, List<Rectangle> RectRemove = null)
        {
            Rectangle bounding = Rectangle.Empty;
            int left = (int)Math.Pow(2, 30);
            int top = (int)Math.Pow(2, 30);
            int bot = 0;
            int right = 0;
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(image, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                if (contours.Size > 0)
                {
                    for (int i = 0; i < contours.Size; i++)
                    {
                        Rectangle bound = CvInvoke.BoundingRectangle(contours[i]);
                        if (RectRemove != null)
                        {
                            if (RectRemove.Contains(bound))
                            {
                                continue;
                            }
                        }
                        left = left > bound.X ? bound.X : left;
                        top = top > bound.Y ? bound.Y : top;
                        right = right < bound.X + bound.Width ? bound.X + bound.Width : right;
                        bot = bot < bound.Y + bound.Height ? bound.Y + bound.Height : bot;
                    }
                    bounding = new Rectangle(left, top, right - left, bot - top);
                }
            }
            return bounding;
        }
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
                using (Image<Gray, byte> imageGray = new Image<Gray, byte>(result.Mask.Size))
                {
                    CvInvoke.CvtColor(result.Mask, imageGray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                    CvInvoke.BitwiseNot(imageGray, imageGray);
                    result.CharBox = GetExtremeBouding(imageGray);
                }
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
        public Rectangle CharBox { get; set; }
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
