using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ARFFBuilder.GUI
{
    /// <summary>
    /// Interaction logic for OpenFileDialog.xaml
    /// </summary>
    public partial class OpenFileDialog : Window
    {
        public string FilePath { get { return tbInputFile.Text; } set { tbInputFile.Text = value; } }
        public OpenFileDialog()
        {
            InitializeComponent();
        }

        private void btnInputFileDialog_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                FilePath = dlg.FileName;
                tbInputFile.Text = FilePath;
            }
        }

        private void tbInputFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilePath = tbInputFile.Text;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
            
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            FilePath = tbInputFile.Text;
            DialogResult = true;
            Close();
        }




    }
}
