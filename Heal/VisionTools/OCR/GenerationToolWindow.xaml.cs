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
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using Emgu.CV.Structure;
using Emgu.CV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heal.VisionTools.OCR
{
    /// <summary>
    /// Interaction logic for GenerationToolWindow.xaml
    /// </summary>
    public partial class GenerationToolWindow : Window, INotifyPropertyChanged
    {
        public Struct.GenConfig mGenConfig = new Struct.GenConfig();
        private System.Timers.Timer mTimerGenerate = new System.Timers.Timer(50);
        public string mSavePath { get; set; }
        public bool mInSetColor = false;
        public bool mIsCancel = false;
        public bool mIsRunning = false;
        public Struct.GenConfig mConfiguration
        {
            get { 
                return mGenConfig;
            }
            set
            {
                mGenConfig = value; 
                OnPropertyChanged();
                
            }
        }
        private Struct.GenConfig LoadConfigFromString(string ConfigStr)
        {
            Struct.GenConfig configLoaded = null;
            try
            {
                configLoaded = JsonConvert.DeserializeObject<Struct.GenConfig>(ConfigStr);
            }
            catch { return null; }
            return configLoaded;
        }
        public GenerationToolWindow()
        {
            mSavePath = "C:\\";
            var config = LoadConfigFromString(Properties.Settings.Default.ConfigChanged);
            if (config != null)
            {
                mConfiguration = config;
            }
            InitializeComponent();
            this.DataContext = this;
        }
        #region Binding to view
        private void SaveConfig(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ConfigChanged = JsonConvert.SerializeObject(mGenConfig);
            Properties.Settings.Default.Save();
        }
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
            //genPage.test(mConfiguration);
            var pageResult = genPage.GetPageImage(mConfiguration);
            if (pageResult != null)
            {
                Log($"Try to preview the process successfully in {sw.ElapsedMilliseconds} ms...!\n");
                for (int i = 0; i < pageResult.BoxChar.Length; i++)
                {
                    System.Drawing.Rectangle box = pageResult.BoxChar[i];
                    CvInvoke.Rectangle(pageResult.ImageGenerated, box, new MCvScalar(0, 0, 255), 1);
                }
                var bms = Struct.Convertor.Bitmap2BitmapSource(pageResult.ImageGenerated.Bitmap);
                imbPreview.Source = bms;
                pageResult.Dispose();
            }
            else
            {
                Log("Try to preview the process failed!\n");
            }
        }
        private void Log(string msg)
        {
            txtLog.Text += msg;
            txtLog.ScrollToEnd();
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

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimerGenerate.Enabled = false;
            this.Dispatcher.Invoke(() =>
            {
                if (cSaveConfig.IsChecked == true)
                {
                    string config = JsonConvert.SerializeObject(mConfiguration);
                    File.WriteAllText($"{mSavePath}/config.json", config);
                }

                for (int pageNum = 0; pageNum < mConfiguration.ImageSetting.ImageNum; pageNum++)
                {
                    try
                    {
                        Utils.PageGeneration genPage = new Utils.PageGeneration();
                        var pageResult = genPage.GetPageImage(mConfiguration);
                        if (pageResult != null)
                        {
                            //Log($"Generate Page  : {pageNum} in {sw.ElapsedMilliseconds} ms...!\n");
                            CvInvoke.Imwrite($"{mSavePath}/Page_{pageNum}.png", pageResult.ImageGenerated);
                            string listBoxStr = string.Empty;
                            for (int i = 0; i < pageResult.BoxChar.Length; i++)
                            {
                                System.Drawing.Rectangle box = pageResult.BoxChar[i];
                                string boxStr = $"((({box.X},{box.Y})," +
                                $"({box.X + box.Width},{box.Y})," +
                                $"({box.X + box.Width}, {box.Y + box.Height})," +
                                $"({box.X}, {box.Y + box.Height})),{pageResult.Char[i]})\n";
                                listBoxStr += boxStr;
                            }
                            File.WriteAllText($"{mSavePath}/Page_{pageNum}.box", listBoxStr);
                            pageResult.Dispose();
                        }
                        else
                        {
                            //Log($"Generate Page  : {pageNum} failed!\n");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log(ex.Message);
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (mIsCancel)
                        break;
                }
                MessageBox.Show("Generate successfully!...", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        private void btGenerate_Click(object sender, RoutedEventArgs e)
        {
            
            string savePath = txtSaveFolder.Text;
            if (Directory.Exists(txtSaveFolder.Text))
            {
                string date = DateTime.Now.ToString("yyyy_MM_dd");
                string savePathChild = $"{savePath}/{date}";
                try
                {
                    Directory.CreateDirectory(savePathChild);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                mSavePath = savePathChild;
                mTimerGenerate.Elapsed += OnTimedEvent;
                mTimerGenerate.Enabled = true;
            }
            else
            {
                MessageBox.Show("Folder is not exists...!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btGenerateEachChar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            if(mIsRunning)
                mIsCancel = true;
        }
    }
}
