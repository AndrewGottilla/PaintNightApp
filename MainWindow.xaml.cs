using Microsoft.Win32;
using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintNight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // List<string> characters = new List<string>();
        public List<string> Characters { get; set; } = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            // Default list of characters
            Characters.Add("test1");
            Characters.Add("test2");
            Characters.Add("test3");
            Characters.Add("test4");
            Characters.Add("test5");
            Characters.Add("test6");
            Characters.Add("test7");
            Characters.Add("test8");
            Characters.Add("test9");
            Characters.Add("test10");

            // Fill list view with characters from list
            fillListView();
        }

        private void fillListView()
        {
            // Get the list view item collection
            ItemCollection ic = lstVwChar.Items;
            
            // Clean slate
            ic.Clear();

            // Loop through the array items
            for (int index = 0; index < Characters.Count; index++)
            {
                // Initialize a new ListViewItem instance
                ListViewItem item = new ListViewItem();

                // Add the content to item
                item.Content = Characters[index];

                // Set the item border as item separator
                item.BorderThickness = new Thickness(0, 0, 0, 1);
                item.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF707FCD"));

                // Finally, add the item to the list view item collection
                ic.Add(item);
            }
        }

        // TODO: Edit list entry

        private void actionOpen(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Environment.CurrentDirectory;
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|Data Files (*.dat)|*.dat";

            // Get the selected file name and grab data
            if (dlg.ShowDialog() == true)
            {
                // Clear list
                Characters.Clear();

                // Open document
                string filename = dlg.FileName;
                StreamReader sr = new StreamReader(filename);

                // Fill characters with items in file
                string line = sr.ReadLine();
                while (line != null)
                {
                    Characters.Add(line);
                    line = sr.ReadLine();
                }

                // Update ListBox to reflect change to characters list
                fillListView();
            }
            // TODO: Else user hits cancel
        }

        private void actionSave(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = Environment.CurrentDirectory;
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|Data Files (*.dat)|*.dat";

            if (Characters.Count == 0) { }
            //TODO: If no items in list, then don't save
            else if (dlg.ShowDialog() == true)
            {
                // TODO: add a try-catch for sw?
                StreamWriter sw = new StreamWriter(dlg.FileName);
                foreach (string c in Characters)
                    sw.WriteLine(c);
                sw.Close();
            }
            else // User chose Cancel or list empty
            {
                // TODO: handle errors
            }
            
        }

        private void actionExit(object sender, RoutedEventArgs e)
        {
            //TODO: Ask to save before closing?
            Application.Current.Shutdown();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            
            // check if null for the case that ListBox is reloaded/unhighlighted
            if (lbi == null)
                lblChar.Content = " - - - ";
            else
                lblChar.Content = lbi.Content.ToString();
        }

        private void addChar()
        {
            // TODO: Implement confirmation window

            // Get the list view item collection
            ItemCollection ic = lstVwChar.Items;

            // Create the new item
            ListViewItem item = new ListViewItem();
            item.Content = txtBxAdd.Text;
            item.BorderThickness = new Thickness(0, 0, 0, 1);
            item.BorderBrush = (SolidColorBrush) (new BrushConverter().ConvertFrom("#FF707FCD"));

                // Finally, add the item to the list view item collection
                ic.Add(item);

                // Add the new item to the character list
                Characters.Add(txtBxAdd.Text);

                //TODO: Add "Character Addition Confirmation" window
        }

        // This function is for pressing enter inside the textbox
        private void eventAddChar(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                addChar();
        }

        // TODO: Implement event for txtBxAdd when hitting Enter \ Rename this event
        private void eventAddChar(object sender, RoutedEventArgs e)
        {
            addChar();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Finish delete definition
            // Open small window asking if you're sure you want to delete "LIST ITEM"
            // If nothing is selected, send simple error message and don't run remova
            Characters.RemoveAt(lstVwChar.SelectedIndex);
            lstVwChar.Items.Remove(lstVwChar.SelectedItem);
        }

        private void actionAbout(object sender, RoutedEventArgs e)
        {
            string message = "- Andrew Gottilla -\n";
            message += "_____ Paint Night _____\n";
            //message += " - Integrated Timer\n";
            message += " - Wrties character list to file\n";
            message += " - Loads character list from file";
            MessageBox.Show(message);
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add animation and sound ? Dice Rolling ?
            Random randy = new Random();
            int num = randy.Next(Characters.Count);
            lstVwChar.SelectedIndex = num;
        }

        private void btnTimer_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Implement Timer Window
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Show previous Google Image
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Show next Google Image
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Copy Google Image SOURCE to Clipboard
        }
    }
}
