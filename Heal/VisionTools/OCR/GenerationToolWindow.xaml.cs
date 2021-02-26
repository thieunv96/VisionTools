using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using Emgu.CV.Structure;
using Emgu.CV;

namespace Heal.VisionTools.OCR
{
    /// <summary>
    /// Interaction logic for GenerationToolWindow.xaml
    /// </summary>
    public partial class GenerationToolWindow : Window, INotifyPropertyChanged
    {
        public Struct.GenConfig mGenConfig = new Struct.GenConfig();
        public string mSavePath { get; set; }
        public bool mInSetColor = false;
        public Struct.GenConfig mConfiguration
        {
            get { return mGenConfig; }
            set
            {
                mGenConfig = value; 
                OnPropertyChanged();
            }
        }
        public GenerationToolWindow()
        {
            mSavePath = "C:\\";
            InitializeComponent();
            this.DataContext = this;
        }
        #region Binding to view
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        private void btTryPreview_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Utils.PageGeneration genPage = new Utils.PageGeneration();
            var imagePage = genPage.GetPageImage(mConfiguration);
            if(imagePage != null)
            {
                txtLog.Text += "Try to preview the process successfully!\n";
                var bms = Struct.Convertor.Bitmap2BitmapSource(imagePage.Bitmap);
                imbPreview.Source = bms;
                imagePage.Dispose();
                imagePage = null;
            }
            else
            {
                txtLog.Text += "Try to preview the process failed!\n";
            }
            Console.WriteLine($"{imbPreview.ActualWidth} x { imbPreview.ActualHeight}");
        }

        private void btSelectTextColor_Click(object sender, RoutedEventArgs e)
        {
            mInSetColor = true;
            using (System.Windows.Forms.ColorDialog colorDlg = new System.Windows.Forms.ColorDialog())
            {
                if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mConfiguration.FontSetting.TextColor = colorDlg.Color;
                    OnPropertyChanged();
                }
            }
            mInSetColor = false;
        }


        private void btSelectBGColor_Click(object sender, RoutedEventArgs e)
        {
            mInSetColor = true;
            using (System.Windows.Forms.ColorDialog colorDlg = new System.Windows.Forms.ColorDialog())
            {
                if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mConfiguration.ImageSetting.BGColor = colorDlg.Color;
                    OnPropertyChanged();
                }
            }
            mInSetColor = false;
        }


        private void txtTextColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!mInSetColor)
            {
                var color = Struct.Action.SetColor(txtBTextColor, txtGTextColor, txtRTextColor);
                if (color != null)
                {
                    mConfiguration.FontSetting.TextColor = (System.Drawing.Color)color;
                    OnPropertyChanged();
                }
            }
        }
        private void txtBGColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!mInSetColor)
            {
                var color = Struct.Action.SetColor(txtBImageColor, txtGImageColor, txtRImageColor);
                if (color != null)
                {
                    mConfiguration.ImageSetting.BGColor = (System.Drawing.Color)color;
                    OnPropertyChanged();
                }
            }
        }

        private void btBrowerFontFolder_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                string path = mConfiguration.FontSetting.FontPath;
                if (Directory.Exists(path))
                {
                    fbd.SelectedPath = path;
                }
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mConfiguration.FontSetting.FontPath = fbd.SelectedPath;
                    OnPropertyChanged();
                }
            }
        }

        private void btSelectFolderBGImage_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                string path = mConfiguration.FontSetting.FontPath;
                if (Directory.Exists(path))
                {
                    fbd.SelectedPath = path;
                }
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mConfiguration.ImageSetting.ImageBGPath = fbd.SelectedPath;
                    OnPropertyChanged();
                }
            }
        }
        private void txtBrowserSaveFolder_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                string path = mSavePath;
                if (Directory.Exists(path))
                {
                    fbd.SelectedPath = path;
                }
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mSavePath = fbd.SelectedPath;
                    OnPropertyChanged();
                }
            }
        }
        #endregion


        private void btGenerate_Click(object sender, RoutedEventArgs e)
        {

            
        }

        
    }
}
