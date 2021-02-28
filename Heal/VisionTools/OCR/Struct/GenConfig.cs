using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

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
        public double _MinFontSize { get; set; }
        public double _MaxFontSize { get; set; }
        public string FontPath { get; set; }
        public FontStyle FontStyle { get; set; }
        public double MinFontSize {
            get => _MinFontSize;
            set
            {
                _MinFontSize = value > 0 ? value : _MinFontSize;
                MaxFontSize = MaxFontSize;
            }
        }
        public double MaxFontSize { 
            get => _MaxFontSize;
            set {
                _MaxFontSize = value < _MinFontSize ? _MinFontSize : value;
            } 
        }
        public int LetterSpacing { get; set; }
        public int LineSpacing { get; set; }
        public Color TextColor { get; set; }
        public FontSt()
        {
            this.MinFontSize = 12.5;
            this.MaxFontSize = 20;
            this.TextColor = Color.Black;
            this.FontStyle = FontStyle.Regular;
            this.FontPath = @"C:\Windows\Fonts";
        }
    }
    public class ImageSt
    {
        public uint _ImageWidth { get; set; }
        public uint _ImageHeight { get; set; }
        public string ImageBGPath { get; set; }
        public uint ImageWidth {
            get => _ImageWidth;
            set
            {
                _ImageWidth = value < 1 ? 1 : value;
            }
        }
        public uint ImageHeight
        {
            get => _ImageHeight;
            set
            {
                _ImageHeight = value < 1 ? 1 : value;
            }
        }
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
            this.ImageBGPath = @"C:\\";
        }
    }
    public class TextSt
    {
        public uint _MinLengthChar { get; set; }
        public uint _MaxLengthChar { get; set; }
        public uint _MinNumberOfLine { get; set; }
        public uint _MaxNumberOfLine { get; set; }
        public double _MinOpacity { get; set; }
        public double _MaxOpacity { get; set; }
        public double MinOpacity {
            get => _MinOpacity;
            set
            {
                _MinOpacity = value < 0 ? 0 : value;
                _MinOpacity = value > 1 ? 1 : value;
                MaxOpacity = MaxOpacity;
            }
        }
        public double MaxOpacity
        {
            get => _MaxOpacity;
            set
            {
                _MaxOpacity = value < _MinOpacity ? _MinOpacity : value;
                _MaxOpacity = _MaxOpacity > 1 ? 1 : _MaxOpacity;
            }
        }
        public bool UseRandomText { get; set; }
        public bool UseImportText { get; set; }
        public uint MinLengthChar {
            get => _MinLengthChar;
            set
            {
                _MinLengthChar = value < 0 ? _MinLengthChar : value;
                MaxLengthChar = MaxLengthChar;
            }
        }
        public uint MaxLengthChar {
            get => _MaxLengthChar;
            set
            {
                _MaxLengthChar = value < _MinLengthChar ? _MinLengthChar : value;
            }
        }
        public uint MinNumberOfLine {
            get => _MinNumberOfLine;
            set
            {
                _MinNumberOfLine = value < 0 ? 0 : value;
                MaxNumberOfLine = MaxNumberOfLine;
            }
        }
        public uint MaxNumberOfLine {
            get => _MaxNumberOfLine;
            set
            {
                _MaxNumberOfLine = value < _MinNumberOfLine ? _MinNumberOfLine : value;
            }
        }
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
            this.MinNumberOfLine = 3;
            this.MaxNumberOfLine = 5;
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
