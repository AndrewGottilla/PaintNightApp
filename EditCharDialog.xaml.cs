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

namespace PaintNight
{
    /// <summary>
    /// Interaction logic for EditCharDialog.xaml
    /// </summary>
    public partial class EditCharDialog : Window
    {
        public string Character { get { return txtBxEdit.Text; } }

        public EditCharDialog(string character)
        {
            InitializeComponent();
            lblCharacter.Content += character;
            txtBxEdit.Text = character;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtBxEdit.SelectAll();
            txtBxEdit.Focus();
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
