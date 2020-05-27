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
using System.Runtime.CompilerServices;
using System.Threading;

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

            // Fill lstVwChar
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
                item.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 112, 127)) { Opacity = 0.5 };

                // Finally, add the item to the list view item collection
                ic.Add(item);
            }
        }

        private void actionOpen(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Environment.CurrentDirectory;
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|Data Files (*.dat)|*.dat";

            // Open file if User did not cancel
            if (dlg.ShowDialog() == true)
            {
                // Clear list
                Characters.Clear();

                // Open document
                StreamReader sr = new StreamReader(dlg.FileName);

                // Fill Characters list with data from file
                string line = sr.ReadLine();
                try
                {
                    while (line != null)
                    {
                        Characters.Add(line);
                        line = sr.ReadLine();
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(String.Format("An error ocurred while executing the data import: {0}", exc.Message), exc);
                }
                finally
                {
                    sr.Close();
                }

                // Update ListBox to reflect change to characters list
                fillListView();
            }
        }

        private void actionSave(object sender, RoutedEventArgs e)
        {
            if (Characters.Count > 0)
            {
                // Create SaveFileDialog
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = Environment.CurrentDirectory;
                sfd.DefaultExt = ".txt";
                sfd.Filter = "Text Files (*.txt)|*.txt|Data Files (*.dat)|*.dat";

                // Create document if User did not cancel
                if (sfd.ShowDialog() == true)
                {
                    // Open document
                    StreamWriter sw = new StreamWriter(sfd.FileName);

                    // Fill document with data from Characters list
                    try
                    {
                        foreach (string c in Characters)
                            sw.WriteLine(c);
                    }
                    catch (Exception exc)
                    {
                        throw new Exception(String.Format("An error ocurred while executing the data import: {0}", exc.Message), exc);
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("The list you're trying to save is empty!", "Paint Night", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void actionExit(object sender, RoutedEventArgs e)
        {
            // TODO: YesNoCancel for saving before closing
            Application.Current.Shutdown();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

            // Check if null for the case that lstVwChar is reloaded/unhighlighted
            if (lbi == null)
                lblChar.Content = " - - - ";
            else
                lblChar.Content = lbi.Content.ToString();
        }

        private void addChar()
        {
            // Get name of input character
            String newChar = txtBxAdd.Text;

            // Check that input character is not blank
            if (!String.IsNullOrEmpty(newChar))
            {
                // Confirmation window
                MessageBoxResult mbr = System.Windows.MessageBox.Show("Are you sure you want to add: \"" + newChar + "\"?", "Character Addition", System.Windows.MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {                     
                    // Get the list view item collection
                    ItemCollection ic = lstVwChar.Items;

                    // Create the new item
                    ListViewItem newCharItem = new ListViewItem();
                    newCharItem.Content = txtBxAdd.Text;
                    newCharItem.BorderThickness = new Thickness(0, 0, 0, 1);
                    newCharItem.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF707FCD"));

                    // Finally, add the item to the list view item collection
                    ic.Add(newCharItem);

                    // Add the new item to the character list
                    Characters.Add(newChar);
                }
            }

            // Clear the TextBox
            txtBxAdd.Text = "";
        }

        // This function is for pressing enter inside the textbox
        private void eventAddChar(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                addChar();
        }

        // This function gets called when you click the add button
        private void eventAddChar(object sender, RoutedEventArgs e)
        {
            addChar();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Grab Item selected in list
            ListBoxItem lbi = ((lstVwChar).SelectedItem as ListBoxItem);

            if (lbi != null)
            {
                // Get name of selected character
                String SelectedCharacter = lbi.Content.ToString();

                // Confirmation window
                MessageBoxResult mbr = System.Windows.MessageBox.Show("Are you sure you want to delete: \"" + SelectedCharacter + "\"?", "Character Deletion", System.Windows.MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    Characters.RemoveAt(lstVwChar.SelectedIndex);
                    lstVwChar.Items.Remove(lstVwChar.SelectedItem);
                }
            }
        }

        private void actionAbout(object sender, RoutedEventArgs e)
        {
            string message = "- Andrew Gottilla -\n";
            message += "_____ Paint Night _____\n";
            //message += " - Integrated Timer\n";
            //message += " - Google Image Integration -\n";
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
            TimerWindow tw = new TimerWindow();
            tw.Show();
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
