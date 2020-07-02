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
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace PaintNight
{
    /// <summary>
    /// Interaction logic for EditTimeDialog.xaml
    /// </summary>
    public partial class EditTimeDialog : Window
    {
        public int Minutes { get { return Convert.ToInt32(txtBxMinutes.Text); } }
        public int Seconds { get { return Convert.ToInt32(txtBxSeconds.Text); } }

        public EditTimeDialog(int m, int s)
        {
            InitializeComponent();

            if (m < 10)
            { txtBxMinutes.Text = "0" + m.ToString(); }
            else
            { txtBxMinutes.Text = m.ToString(); }

            if (s < 10)
            { txtBxSeconds.Text = "0" + s.ToString(); }
            else
            { txtBxSeconds.Text = s.ToString(); }
        }

        // Disables keyboard commands
        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut || e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        // Only allows numbers to be input into textbox
        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
