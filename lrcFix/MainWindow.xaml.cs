using System;
using System.Windows;
using System.Windows.Controls;

namespace lrcFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime Stopwatch;

        public MainWindow()
        {
            InitializeComponent();

            PathTextBox.AddHandler(TextBox.DragOverEvent, new DragEventHandler(PathTextBox_DragOver), true);
            PathTextBox.AddHandler(TextBox.DropEvent, new DragEventHandler(PathTextBox_Drop), true);
        }

        private void PathTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }

        private void PathTextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                PathTextBox.Text = string.Empty;
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
                PathTextBox.Text = filenames[0];
            }
        }

        private void StopwatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (StopwatchButton.Content.ToString() == "Stopwatch")
            {
                Stopwatch = DateTime.Now;
                StopwatchButton.Content = "Stop";
            }
            else
            {
                TimeSpan time = (DateTime.Now - Stopwatch);
                OffsetMinTextBox.Text = String.Format("{0:00}", time.Minutes);
                OffsetSecTextBox.Text = String.Format("{0:00}", time.Seconds);
                OffsetMilliSecTextBox.Text = String.Format("{0:00}", time.Milliseconds.ToString().PadRight(2, '0').Substring(0, 2));
                StopwatchButton.Content = "Stopwatch";
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int min = int.Parse(OffsetMinTextBox.Text);
            int sec = int.Parse(OffsetSecTextBox.Text);
            int milliSec = int.Parse(OffsetMilliSecTextBox.Text + "0");
            TimeSpan offsetTime = new TimeSpan(0, 0, min, sec, milliSec);
            Processing.FixLrcFile(PathTextBox.Text, FasterRadio.IsChecked.Value,offsetTime);
            MessageBox.Show("Success!", "OK");
        }
    }
}
