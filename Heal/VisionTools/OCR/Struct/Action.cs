using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Heal.VisionTools.OCR.Struct
{
    class Action
    {
        public static object SetColor(TextBox txtB, TextBox txtG, TextBox txtR)
        {
            int b = 0;
            int g = 0;
            int r = 0;
            try
            {
                b = Convert.ToInt32(txtB.Text);
            }
            catch (Exception)
            {
                txtB.BorderBrush = Brushes.Red;
                return null;
            }
            try
            {
                g = Convert.ToInt32(txtG.Text);
            }
            catch (Exception)
            {
                txtG.BorderBrush = Brushes.Red;
                return null;
            }
            try
            {
                r = Convert.ToInt32(txtR.Text);
            }
            catch (Exception)
            {
                txtR.BorderBrush = Brushes.Red;
                return null;
            }
            if (b >= 0 && b < 256 && g >= 0 && g < 256 && r >= 0 && r < 256)
            {
                txtB.BorderBrush = Brushes.Gray;
                txtG.BorderBrush = Brushes.Gray;
                txtR.BorderBrush = Brushes.Gray;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
                return color;
            }
            else
            {
                if (b < 0 || b >= 256)
                    txtB.BorderBrush = Brushes.Red;
                if (g < 0 || g >= 256)
                    txtG.BorderBrush = Brushes.Red;
                if (r < 0 || r >= 256)
                    txtR.BorderBrush = Brushes.Red;
                return null;
            }
        }
    }
}
