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
        private int time = 300;
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
        }

        private void printTime()
        {
            TimeSpan realTime = TimeSpan.FromSeconds(time);
            lblTimer.Content = realTime.ToString(@"\mm\:ss");
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            // Counts down
            time--;
            printTime();
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (ticking)
            {
                dt.Stop();
                ticking = false;
                btnPlayPause.Content = "Resume";
                btnReset.IsEnabled = true;
            }
            else
            {
                dt.Start();
                ticking = true;
                btnPlayPause.Content = "Pause";
                btnReset.IsEnabled = false;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (ticking)
            {
                dt.Stop();
                ticking = false;
                btnPlayPause.Content = "Start";
                btnReset.IsEnabled = true;
            }
            else
            {
                time = 300;
                printTime();
            }
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

    }
}
