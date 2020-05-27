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
        private int increment = 300;
        public TimerWindow()
        {
            InitializeComponent();

            TimeSpan realTime = TimeSpan.FromSeconds(increment);
            lblTimer.Content = realTime.ToString(@"mm\:ss");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += Dt_Tick;
            dt.Start();
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            increment--;
            TimeSpan realTime = TimeSpan.FromSeconds(increment);
            lblTimer.Content = realTime.ToString(@"mm\:ss");

        }
    }
}
