using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ImageConversion.WPF
{
    public partial class FiltrationPage : Page
    {
        private static readonly Regex _regex = new("[^0-9.-]+");

        public FiltrationPage()
        {
            InitializeComponent();
        }

        private void MaskValue_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsNumberAllowed(e.Text);
        }

        private static bool IsNumberAllowed(string text)
        {
            return _regex.IsMatch(text);
        }

        private void FiltrationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentFiltration = (ComboBoxItem)FiltrationComboBox.SelectedItem;

            MaskComboBox.IsEnabled = currentFiltration.Name == "SmoothingFilterComboBoxItem";
        }

        private void MaskComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentMask = (ComboBoxItem)MaskComboBox.SelectedItem;

            for (var i = 0; i < 9; ++i)
            {
                var maskTextBox = (TextBox)FindName($"Mask{i}Value");
                maskTextBox.IsEnabled = (string)currentMask.Content == "Manual mask";
            }

            switch ((string)currentMask.Content)
            {
                case "Smoothing":
                    for (var i = 0; i < 9; ++i)
                    {
                        var maskTextBox = (TextBox)FindName($"Mask{i}Value");
                        maskTextBox.Text = "1";
                    }

                    break;

                case "Previtt horizontal": SetMaskValues(
                    1, 1, 1, 
                    0, 0, 0, 
                    -1, -1, -1); break;

                case "Previtt vertical": SetMaskValues(
                    -1, 0, 1, 
                    -1, 0, 1, 
                    -1, 0, 1); break;

                case "Sobel horizontal": SetMaskValues(
                    1, 2, 1, 
                    0, 0, 0, 
                    -1, -2, -1); break;

                case "Sobel vertical": SetMaskValues(
                    -1, 0, 1, 
                    -2, 0, 2, 
                    -1, 0, 1); break;

                case "Laplace high frequencies": SetMaskValues(
                    -1, -1, -1, 
                    -1, 8, -1, 
                    -1, -1, -1); break;

                case "Gauss low frequencies": SetMaskValues(
                    1, 2, 1, 
                    2, 4, 2, 
                    1, 2, 1); break;

                case "Sharpening": SetMaskValues(
                    -1, -1, -1, 
                    -1, 16, -1, 
                    -1, -1, -1); break;

                default: break;
            }
        }

        private void SetMaskValues(params int[] values)
        {
            Mask0Value.Text = values[0].ToString();
            Mask1Value.Text = values[1].ToString();
            Mask2Value.Text = values[2].ToString();
            Mask3Value.Text = values[3].ToString();
            Mask4Value.Text = values[4].ToString();
            Mask5Value.Text = values[5].ToString();
            Mask6Value.Text = values[6].ToString();
            Mask7Value.Text = values[7].ToString();
            Mask8Value.Text = values[8].ToString();
        }
    }
}
