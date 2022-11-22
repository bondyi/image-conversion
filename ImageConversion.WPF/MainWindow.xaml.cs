using ImageConversion.Algorithms;
using ImageConversion.Algorithms.Filtration;
using ImageConversion.Algorithms.Rotation;
using ImageConversion.Algorithms.Scale;
using ImageConversion.WPF;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageConversion
{
    public partial class MainWindow : Window
    {
        private string filePath = "SavedImage.png";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConversionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentConversion = (ComboBoxItem)ConversionComboBox.SelectedItem;
            ConversionSettingsFrame.Source = new Uri($"{currentConversion.Content + "Page"}.xaml", UriKind.Relative);
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png *.jpg) |*.png; *.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                CurrentImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                var fileInfo = new FileInfo(filePath);

                ResolutionBeforeTextBlock.Text = Math.Round(CurrentImage.Source.Width).ToString() + 'x' + Math.Round(CurrentImage.Source.Height).ToString();
                SizeBeforeTextBlock.Text = fileInfo.Length.ToString();
                FormatBeforeTextBlock.Text = fileInfo.Extension;

                StartButton.IsEnabled = true;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var image = new Bitmap(filePath);

            var frameContent = ConversionSettingsFrame.Content;

            var stopwatch = new Stopwatch();

            switch (ConversionComboBox.Text)
            {
                case "Scale":
                    var scalePage = (ScalePage)frameContent;
                    var scaleX = (float)scalePage.ScaleXSlider.Value;
                    var scaleY = (float)scalePage.ScaleYSlider.Value;

                    stopwatch.Start();
                    image = scalePage.AlgorithmComboBox.Text switch
                    {
                        "Nearest neighbor" => new NearestNeighbor().Scale(image, scaleX, scaleY),
                        "Increase in K times" => new IncreaseInKTimes().Scale(image, scaleX, scaleY),
                        "Bilinear interpolation" => new BilinearInterpolation().Scale(image, scaleX, scaleY),
                        _ => throw new NotImplementedException(),
                    };
                    stopwatch.Stop();

                    break;


                case "Rotation":
                    var rotationPage = (RotationPage)frameContent;
                    var angle = (int)rotationPage.RotationSlider.Value;

                    stopwatch.Start();
                    image = new FastRotation().Rotate(image, angle);
                    stopwatch.Stop();
                    
                    break;


                case "Filtration":
                    var filtrationPage = (FiltrationPage)frameContent;

                    switch (filtrationPage.FiltrationComboBox.Text)
                    {
                        case "Median filter":
                            stopwatch.Start();
                            image = new MedianFilter().Filtrate(image, new double[0, 0]);
                            stopwatch.Stop();

                            break;

                        case "Manual filter":
                            var mask = new double[3, 3];

                            var count = 0;
                            for (var i = 0; i < 3; ++i)
                            {
                                for (var j = 0; j < 3; ++j)
                                {
                                    var maskTextBox = (TextBox)filtrationPage.FindName($"Mask{count++}Value");
                                    mask[i, j] = Convert.ToDouble(maskTextBox.Text);
                                }
                            }

                            stopwatch.Start();
                            image = new ManualFilter().Filtrate(image, mask);
                            stopwatch.Stop();

                            break;
                            
                        default: return;
                    }
                    break;

                default: return;
            }

            CurrentImage.Source = Algorithm.ToBitmapImage(image);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)CurrentImage.Source));
            using (var stream = new FileStream("SavedImage.png", FileMode.Create))
            encoder.Save(stream);

            var fileInfo = new FileInfo($"SavedImage.png");
            ResolutionAfterTextBlock.Text = Math.Round(CurrentImage.Source.Width).ToString() + 'x' + Math.Round(CurrentImage.Source.Height).ToString();
            SizeAfterTextBlock.Text = fileInfo.Length.ToString();
            FormatAfterTextBlock.Text = fileInfo.Extension;
            TimeValueTextBlock.Text = stopwatch.Elapsed.ToString();
        }
    }
}
