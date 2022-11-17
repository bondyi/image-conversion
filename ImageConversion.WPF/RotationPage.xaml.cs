using System.Windows;
using System.Windows.Controls;

namespace ImageConversion
{
    public partial class RotationPage : Page
    {
        public RotationPage()
        {
            InitializeComponent();
        }

        private void RotationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotationValue.Text = RotationSlider.Value.ToString() + '°';
        }
    }
}
