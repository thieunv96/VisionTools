using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Heal.VisionTools.OCR.Struct
{
    public class GenConfig
    {
        public FontSt FontSetting { get; set; }
        public ImageSt ImageSetting { get; set; }
        public TextSt TextSetting { get; set; }
        public EffectSt EffectcSetting { get; set; }
        public GenConfig()
        {
            this.FontSetting = new FontSt();
            this.ImageSetting = new ImageSt();
            this.TextSetting = new TextSt();
            this.EffectcSetting = new EffectSt();
        }
    }
    public class FontSt
    {
        public string FontPath { get; set; }
        public double MinFontSize { get; set; }
        public double MaxFontSize { get; set; }
        public uint LetterSpacing { get; set; }
        public uint LineSpacing { get; set; }
        public Color TextColor { get; set; }
        public FontSt()
        {

            this.MinFontSize = 12.5;
            this.MaxFontSize = 20;
            this.TextColor = Color.Black;
            this.FontPath = @"D:\Projects\MrDuong\VisionTools\test\font";
        }
    }
    public class ImageSt
    {
        public string ImageBGPath { get; set; }
        public uint ImageWidth { get; set; }
        public uint ImageHeight { get; set; }
        public uint ImageNum { get; set; }
        public bool UseBGFromImage { get; set; }
        public bool UseBGFromColor { get; set; }
        public Color BGColor { get; set; }
        public ImageSt()
        {
            this.ImageWidth = 500;
            this.ImageHeight = 500;
            this.ImageNum = 10;
            this.UseBGFromColor = true;
            this.UseBGFromImage = false;
            this.BGColor = Color.White;
            this.ImageBGPath = @"D:\Projects\MrDuong\VisionTools\test\background";
        }
    }
    public class TextSt
    {
        public double MinOpacity { get; set; }
        public double MaxOpacity { get; set; }
        public bool UseRandomText { get; set; }
        public bool UseImportText { get; set; }
        public uint MinLengthChar { get; set; }
        public uint MaxLengthChar { get; set; }
        public bool UseNumber { get; set; }
        public bool UseUpperChar { get; set; }
        public bool UseLowerChar { get; set; }
        public bool UseOtherChar { get; set; }
        public string OtherChar { get; set; }
        public string ImportText { get; set; }
        public TextSt()
        {
            this.MinOpacity = 0.7;
            this.MaxOpacity = 1.0;
            this.MinLengthChar = 6;
            this.MaxLengthChar = 8;
            this.UseRandomText = true;
            this.UseImportText = false;
            this.UseNumber = true;
            this.UseUpperChar = false;
            this.UseLowerChar = false;
            this.UseOtherChar = false;
            this.ImportText = "123\nABC\nabc";
        }
    }
    public class EffectSt
    {
        public bool UseAverageBlur { get; set; }
        public bool UseMedianBlur { get; set; }
        public bool UseGaussianBlur { get; set; }
        public bool UsePepperNoise { get; set; }
        public bool UseGaussianNoise { get; set; }
        public EffectSt()
        {
            this.UseAverageBlur = true;
            this.UseMedianBlur = true;
            this.UseGaussianBlur = true;
            this.UseGaussianNoise = true;
            this.UsePepperNoise = true;
        }
    }
}
