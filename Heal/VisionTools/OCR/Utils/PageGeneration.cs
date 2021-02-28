using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Heal.VisionTools.OCR.Utils
{
    class PageGeneration
    {
        private static System.Drawing.Text.PrivateFontCollection mFontCollection = new System.Drawing.Text.PrivateFontCollection();
        public Image<Bgr, byte> GetPageImage(Struct.GenConfig Config)
        {
            Image<Bgr, byte> imageOutput = InitImage(Config.ImageSetting);
            if(imageOutput != null)
            {
                Random random = new Random();
                int xSt = random.Next(0, imageOutput.Width);
                int ySt = random.Next(0, imageOutput.Height);
                int numLine = random.Next((int)Config.TextSetting.MinNumberOfLine, (int)Config.TextSetting.MaxNumberOfLine + 1);
                Rectangle ROISt = new Rectangle(xSt, ySt, imageOutput.Width - xSt, imageOutput.Height - ySt);
                imageOutput.ROI = ROISt;
                Font font = GetFont(Config.FontSetting);
                using (Image<Bgr, byte> image = imageOutput.Copy())
                {
                    if (font != null)
                    {
                        int heightChar = GetHeightOneChar(font);
                        string[] content = GetArrayContent(Config.TextSetting, heightChar, (int)Config.ImageSetting.ImageHeight);
                        Utils.DrawCharacter draw = new DrawCharacter();
                        for (int row = 0; row < content.Length; row++)
                        {
                            if (Config.TextSetting.UseRandomText)
                                if (row > numLine - 1)
                                    break;
                            int x = 0;
                            for (int col = 0; col < content[row].Length; col++)
                            {
                                string chr = content[row][col].ToString();
                                DrawResult result = draw.DrawText(chr, font, Config.FontSetting.TextColor);
                                Rectangle ROI = new Rectangle(x, (int)(row * (heightChar + 0.2 * heightChar)), result.Image.Width, result.Image.Height);
                                if (ROI.X >= image.Width || ROI.Y >= image.Height)
                                    break;
                                image.ROI = ROI;
                                if (image.Width < result.Image.Width || image.Height < result.Image.Height)
                                {
                                    Rectangle ROIResult = new Rectangle(0, 0, image.Width, image.Height);
                                    result.Image.ROI = ROIResult;
                                    result.Mask.ROI = ROIResult;
                                }
                                using (Image<Bgr, byte> imageAdd = new Image<Bgr, byte>(result.Image.Size))
                                {
                                    CvInvoke.BitwiseAnd(image, result.Mask, image);
                                    CvInvoke.Add(result.Image, image, imageAdd);
                                    imageAdd.CopyTo(image);
                                }
                                image.ROI = Rectangle.Empty;
                                x += result.Image.Width;
                            }
                        }
                    }
                    image.CopyTo(imageOutput);
                }

                imageOutput.ROI = Rectangle.Empty;
            }
            return imageOutput;
        }
        public string[] GetArrayContent(Struct.TextSt textSt, int heightChar, int heightImage)
        {
            List<string> content = new List<string>();
            double inflateHeightChar = (heightChar + 0.2 * heightChar);
            int num = heightImage / (int)inflateHeightChar;
            if (textSt.UseRandomText)
            {
                string listChar = string.Empty;
                if(textSt.UseNumber)
                {
                    for (int i = 48; i < 58; i++)
                    {
                        listChar += (char)i;
                    }
                }
                if (textSt.UseUpperChar)
                {
                    for (int i = 65; i < 91; i++)
                    {
                        listChar += (char)i;
                    }
                }
                if (textSt.UseLowerChar)
                {
                    for (int i = 97; i < 123; i++)
                    {
                        listChar += (char)i;
                    }
                }
                if(textSt.UseOtherChar && textSt.OtherChar != null)
                {
                    listChar += textSt.OtherChar;
                }
                listChar.Replace(" ", "");
                Random random = new Random();
                for (int i = 0; i < num; i++)
                {
                    string str = string.Empty;
                    int length = random.Next((int)textSt.MinLengthChar, (int)textSt.MaxLengthChar);
                    for (int c = 0; c < length; c++)
                    {
                        int idChar = random.Next(0, listChar.Length);
                        str += listChar[idChar];
                    }
                    content.Add(str);
                }
            }
            else if (textSt.UseImportText)
            {
                if (!string.IsNullOrEmpty(textSt.ImportText))
                {
                    string text = textSt.ImportText;
                    content.AddRange(text.Split('\n'));
                }
            }
            return content.ToArray();
        }
        public int GetHeightOneChar(Font font)
        {
            SizeF textSize = new SizeF(1, 1);
            using (Image img = new Bitmap(1, 1))
            using (Graphics temp = Graphics.FromImage(img))
            {
                textSize = temp.MeasureString("A", font);
            }
            return (int)textSize.Width;
        }
        public Font GetFont(Struct.FontSt fontSt)
        {
            Font font = null;
            string[] listFont = Directory.GetFiles(fontSt.FontPath, "*.ttf");
            if(listFont.Length > 0)
            {
                Random random = new Random();
                int id = random.Next(0, listFont.Length);
                if(mFontCollection != null)
                {
                    mFontCollection.Dispose();
                    mFontCollection = null;
                }
                mFontCollection = new System.Drawing.Text.PrivateFontCollection();
                mFontCollection.AddFontFile(listFont[id]);
                using (FontFamily fontFamily = new FontFamily(mFontCollection.Families[0].Name, mFontCollection))
                {
                    int fontSize = random.Next(Convert.ToInt32(fontSt.MinFontSize * 10), Convert.ToInt32((fontSt.MaxFontSize) * 10));
                    font = new Font(fontFamily, (float)(fontSize) / 10, fontSt.FontStyle);
                }
                
            }
            return font;
        }
        public Image<Bgr, byte> InitImage(Struct.ImageSt imageSt)
        {
            Image<Bgr, byte> image = null;
            if (imageSt.UseBGFromColor)
            {
                image = new Image<Bgr, byte>((int)imageSt.ImageWidth, (int)imageSt.ImageHeight, new Bgr(imageSt.BGColor.B, imageSt.BGColor.G, imageSt.BGColor.R));
            }
            else if (imageSt.UseBGFromImage)
            {
                image = new Image<Bgr, byte>((int)imageSt.ImageWidth, (int)imageSt.ImageHeight);
                List<string> bgFilePath = new List<string>();
                string[] ext = new string[] { "*.png", "*.jpg", "*.bmp", "*.jpeg", "*.tiff" };
                foreach (var item in ext)
                {
                    bgFilePath.AddRange(Directory.GetFiles(imageSt.ImageBGPath, item));
                }
                if(bgFilePath.Count > 0)
                {
                    Random random = new Random();
                    int id = random.Next(0, bgFilePath.Count);
                    using (Image<Bgr, byte> imgBg = new Image<Bgr, byte>(bgFilePath[id]))
                    {
                        int stepw = image.Width % imgBg.Width == 0 ? image.Width / imgBg.Width : image.Width / imgBg.Width + 1;
                        int steph = image.Height % imgBg.Height == 0 ? image.Height / imgBg.Height : image.Height / imgBg.Height + 1;
                        for (int x = 0; x < stepw; x++)
                        {
                            for (int y = 0; y < steph; y++)
                            {
                                Rectangle ROIDrawImage = new Rectangle(new Point(x* imgBg.Width, y* imgBg.Height),imgBg.Size);
                                image.ROI = ROIDrawImage;
                                if(image.Width < imgBg.Width || image.Height < imgBg.Height)
                                {
                                    imgBg.ROI = new Rectangle(new Point(0,0), image.Size);
                                }
                                imgBg.CopyTo(image);
                                imgBg.ROI = Rectangle.Empty;
                                image.ROI = Rectangle.Empty;
                            }
                        }
                    }
                }
            }
            return image;
        }
    }
}
