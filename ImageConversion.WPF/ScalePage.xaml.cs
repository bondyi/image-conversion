using System;
using System.Windows;
using System.Windows.Controls;

namespace ImageConversion
{
    public partial class ScalePage : Page
    {
        public ScalePage()
        {
            InitializeComponent();
        }

        private void ScaleXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScaleXValue.Text = Math.Round(ScaleXSlider.Value, 2).ToString();
        }

        private void ScaleYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScaleYValue.Text = Math.Round(ScaleYSlider.Value, 2).ToString();
        }
    }
}
