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

namespace Heal.VisionTools.OCR
{
    /// <summary>
    /// Interaction logic for GenerationToolWindow.xaml
    /// </summary>
    public partial class GenerationToolWindow : Window
    {
        public GenerationToolWindow()
        {
            InitializeComponent();
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
