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
        // List<string> CharacterList = new List<string>();
        public List<string> CharacterList { get; set; } = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            // Default list of CharacterList
            for (int i = 0; i <= 11; i++)
            {
                CharacterList.Add("test" + i);
            }

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
            for (int index = 0; index < CharacterList.Count; index++)
            {
                // Initialize a new ListViewItem instance
                ListViewItem item = new ListViewItem();

                // Add the content to item
                item.Content = CharacterList[index];

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
                CharacterList.Clear();

                // Open document
                StreamReader sr = new StreamReader(dlg.FileName);

                // Fill CharacterList with data from file
                string line = sr.ReadLine();
                try
                {
                    while (line != null)
                    {
                        CharacterList.Add(line);
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

                // Update ListBox to reflect change to CharacterList
                fillListView();
            }
        }

        private void actionSave(object sender, RoutedEventArgs e)
        {
            if (CharacterList.Count > 0)
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

                    // Fill document with data from CharacterList
                    try
                    {
                        foreach (string c in CharacterList)
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
                System.Windows.MessageBox.Show("The list you're trying to save is empty!", "Character Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void actionAbout(object sender, RoutedEventArgs e)
        {
            string message = "- Andrew Gottilla -\n";
            message += "This is a program I made to help with a fun activity that I've created for my friends and I.\n";
            message += "We hang out on Discord, fill a list with a bunch of characters (including ourselves),\n";
            message += "then randomly have a character chosen. We then use our computers to create/draw/edit\n";
            message += "that character! We typically set a 5 or 10 minute timer. I call it 'Paint Night'!";
            MessageBox.Show(message);
        }

        private void actionExit(object sender, RoutedEventArgs e)
        {
            // TODO: YesNoCancel for saving before closing
            // TODO: Close Timer window
            Application.Current.Shutdown();
        }

        // This function gets called when you click the delete button
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Grab Item selected in list
            ListBoxItem lbi = lstVwChar.SelectedItem as ListBoxItem;

            if (lbi != null)
            {
                // Get name of selected character
                String SelectedCharacter = lbi.Content.ToString();

                // Confirmation window
                MessageBoxResult mbr = System.Windows.MessageBox.Show("Are you sure you want to delete: \"" + SelectedCharacter + "\"?", "Character Deletion", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                {
                    CharacterList.RemoveAt(lstVwChar.SelectedIndex);
                    lstVwChar.Items.Remove(lstVwChar.SelectedItem);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a character first", "Paint Night", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Grab Item selected in list
            ListBoxItem lbi = lstVwChar.SelectedItem as ListBoxItem;

            if (lbi != null)
            {
                // Get name of selected character
                String SelectedCharacter = lbi.Content.ToString();

                // Open EditCharDialog window
                EditCharDialog ecd = new EditCharDialog(SelectedCharacter);
                if (ecd.ShowDialog() == true)
                {
                    lbi.Content = ecd.Character;
                    lblChar.Content = ecd.Character;
                    CharacterList[lstVwChar.SelectedIndex] = ecd.Character;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a character first", "Paint Night", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // This function gets called when you click the add button
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            addChar();
        }

        private void addChar()
        {
            // Get name of input character
            String newChar = txtBxAdd.Text;

            // Check that input character is not blank
            if (!String.IsNullOrEmpty(newChar) && !newChar.Equals("Add a new character . . ."))
            {
                // Confirmation window
                MessageBoxResult mbr = System.Windows.MessageBox.Show("Are you sure you want to add: \"" + newChar + "\"?", "Character Addition", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                {
                    // Get the list view item collection
                    ItemCollection ic = lstVwChar.Items;

                    // Create the new item
                    ListViewItem newCharItem = new ListViewItem();
                    newCharItem.Content = txtBxAdd.Text;
                    newCharItem.BorderThickness = new Thickness(0, 0, 0, 1);
                    newCharItem.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 112, 127)) { Opacity = 0.5 };

                    // Finally, add the item to the list view item collection
                    ic.Add(newCharItem);

                    // Add the new item to the character list
                    CharacterList.Add(newChar);

                    // Clear the TextBox
                    txtBxAdd.Text = "";
                    lstVwChar.ScrollIntoView(newCharItem);
                }
                else
                {
                    txtBxAdd.SelectAll();
                    txtBxAdd.Focus();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter a valid character first", "Paint Night", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Keyboard.Focus(txtBxAdd);
        }

        // Function for txtBxAdd 'Hint' (1/2)
        private void txtBxAdd_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtBxAdd.Text == "Add a new character . . .")
            {
                txtBxAdd.Text = "";
                txtBxAdd.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE8E8E8");
            }
        }

        // Function for txtBxAdd 'Hint' (2/2)
        private void txtBxAdd_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtBxAdd.Text))
            {
                txtBxAdd.Text = "Add a new character . . .";
                txtBxAdd.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFBDBDBD");
            }
        }

        // This function is for pressing Enter inside the textbox
        private void txtBxAdd_Shortcuts(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                addChar();
            else if (e.Key == Key.Escape)
            {
                txtBxAdd.Text = "";
                Keyboard.ClearFocus();
            }
        }

        // TODO: Add animation and sound ? Dice Rolling ?
        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            // Generate random number based on length of character lise
            Random randy = new Random();
            int num = randy.Next(CharacterList.Count);

            // Choose character in list based on random number
            lstVwChar.SelectedIndex = num;
            lstVwChar.ScrollIntoView(lstVwChar.SelectedItem);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lstVwChar.SelectedItem = null;
        }

        private void btnTimer_Click(object sender, RoutedEventArgs e)
        {
            TimerWindow tw = new TimerWindow();
            tw.Show();
        }

        private void lstVwChar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

            // Check if null for the case that lstVwChar is reloaded/unhighlighted
            if (lbi == null)
                lblChar.Content = " - - - ";
            else
                lblChar.Content = lbi.Content.ToString();
        }

        // Prevents holding LeftMouseDown issue in lstVwChar
        private void lstVwChar_MouseMove(object sender, MouseEventArgs e)
        {
            if (lstVwChar.IsMouseCaptured)
                lstVwChar.ReleaseMouseCapture();
        }

        // ==================================================================================================================

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
