using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;

namespace PaintNight
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TimerWindow : Window
    {
        public static TimerWindow Current { get; private set; } //singleton
        private DispatcherTimer dt = new DispatcherTimer();
        private int time = 10;
        private bool ticking = false;

        public TimerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Current != null)
            {
                // if old instance exists, close the new instance
                this.Close();

                String errMsg = "Timer is already active!\nPress OK to go to the Timer Window";
                System.Windows.MessageBox.Show(errMsg, "Paint Night", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);

                if (!Current.IsActive)
                    Current.Activate();

                if (!Current.IsFocused)
                    Current.Focus();
            }
            else
            {
                // a new instance
                Current = this;
            }

            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += Dt_Tick;
            printTime();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // If user closes the window -- this is the new instance, Current is the old one
            if ((Current == this) && (Current != null))
                Current = null;

            dt.Stop();
        }

        private void printTime()
        {
            TimeSpan realTime = TimeSpan.FromSeconds(time);
            lblTimer.Content = realTime.ToString(@"mm\:ss");
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            // Counts down
            time--;
            printTime();
            if (time == 0)
            {
                soundTheAlarm();
            }
        }

        private void soundTheAlarm()
        {
            dt.Stop();
            ticking = false;
            btnPlayPause.IsEnabled = false;
            btnReset.IsEnabled = true;
            btnSetTime.IsEnabled = true;
            try //FileNotFoundException
            {
                SoundPlayer sp = new SoundPlayer(Environment.CurrentDirectory + "\\Resources\\exit_cue_y.wav");
                sp.Play();
            }
            catch
            {
                System.Windows.MessageBox.Show("Error!\n" + Environment.CurrentDirectory + "\\Resources\\exit_cue_y.wav not found!", "Paint Night", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (time != 0)
            {
                if (ticking) // Pause
                {
                    dt.Stop();
                    ticking = false;
                    btnPlayPause.Content = "Resume";
                    btnReset.IsEnabled = true;
                    btnSetTime.IsEnabled = true;
                }
                else // Play/Resume
                {
                    dt.Start();
                    ticking = true;
                    btnPlayPause.Content = "Pause";
                    btnReset.IsEnabled = false;
                    btnSetTime.IsEnabled = false;
                }
            }
            
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            btnPlayPause.IsEnabled = true;
            btnPlayPause.Content = "Start";
            time = 300;
            printTime();
        }

        private void btnAddThirty_Click(object sender, RoutedEventArgs e)
        {
            time += 30;
            printTime();
        }

        private void btnAddMin_Click(object sender, RoutedEventArgs e)
        {
            time += 60;
            printTime();
        }

        private void btnAddFive_Click(object sender, RoutedEventArgs e)
        {
            time += 300;
            printTime();
        }

        private void btnSetTime_Click(object sender, RoutedEventArgs e)
        {
            EditTimeDialog etd = new EditTimeDialog(time / 60, time % 60);
            if (etd.ShowDialog() == true)
            {
                time = etd.Minutes * 60 + etd.Seconds;
                printTime();
            }
        }
    }
}
