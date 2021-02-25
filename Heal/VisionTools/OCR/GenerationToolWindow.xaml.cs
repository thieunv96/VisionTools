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

namespace Heal.VisionTools.OCR
{
    /// <summary>
    /// Interaction logic for GenerationToolWindow.xaml
    /// </summary>
    public partial class GenerationToolWindow : Window, INotifyPropertyChanged
    {
        public Struct.GenConfig mConfiguration = new Struct.GenConfig();
        public string   test = "test";
        public GenerationToolWindow()
        {
            InitializeComponent();
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private void InitBinding()
        {
            this.DataContext = this;
        }
        private void txtLetterSpaceing_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int val = Convert.ToInt32((sender as TextBox).Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                (sender as TextBox).Foreground = Brushes.Red;
            }
        }

        private void txtLetterSpaceing_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                txtLetterSpaceing_LostFocus(sender, null);
            }
        }
    }
}
