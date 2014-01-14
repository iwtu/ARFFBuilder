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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ARFFBuilder.Entity;
using System.Windows.Threading;
using System.IO;
using System.Threading;
using System.Globalization;

namespace ARFFBuilder.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings previousSettings;
        Settings settings = new Settings();
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Loaded last settings per user account.
        /// </summary>
        private void LoadSettings()
        {
            var ds = Properties.Settings.Default;
            tbInputTrainFile.Text = ds.InputTrainFile;            
            tbDiagnoses.Text = ds.Diagnoses;
            chmStemmer.IsChecked = ds.UseStemmer;
            chmPunctuation.IsChecked = ds.IgnorePunctuation;
            chmStopWords.IsChecked = ds.IgnoreStopWords;
            tbTopWordsCount.Text = ds.TopWordsCount;
            tbTopWordsFrequency.Text = ds.TopWordsFrequency;
            tbBigramsCount.Text = ds.BigramsCount;
            tbBigramsFrequency.Text = ds.BigramsFrequency;
            tbOuputFile.Text = ds.OutputFile;
            chbFeaturesLoad.IsChecked = ds.LoadFeature;
            settings.Preprocessing.StopwordsInput = ds.StopWordsFile;
            settings.FeaturesFile = ds.FeaturesFile;
            settings.Preprocessing.StopWordsIDFCount = 200; //FIXME: C
        }

        /// <summary>
        /// Saves the least settings.
        /// </summary>
        private void SaveSettings()
        {
            settings.ReportsFile = tbInputTrainFile.Text;            
            settings.Diagnosis = tbDiagnoses.Text;
            settings.LoadFeatures = chbFeaturesLoad.IsChecked.Value;            
            settings.ARFFFile = tbOuputFile.Text;
            settings.Preprocessing.UseCzechStemmer = chmStemmer.IsChecked;
            settings.Preprocessing.FilterStopWords = chmStopWords.IsChecked;
            settings.Preprocessing.IgnorePunctuation = chmPunctuation.IsChecked;
            settings.Features.Clear();
            if (chbBigrams.IsChecked.Value) settings.Features.Add(FeaturesEnum.Bigrams, new Settings.Feature(FeaturesEnum.Bigrams, Convert.ToInt32(tbBigramsCount.Text), Convert.ToInt32(tbBigramsFrequency.Text)));
            if (chbTopWords.IsChecked.Value) settings.Features.Add(FeaturesEnum.Unigrams, new Settings.Feature(FeaturesEnum.Unigrams, Convert.ToInt32(tbTopWordsCount.Text), Convert.ToInt32(tbTopWordsFrequency.Text)));
            
            settings.CheckChanges(previousSettings);
            previousSettings = settings.CopySettings();
            settings.PMI = chmPMI.IsChecked;
            settings.Preprocessing.StopwordsIDF = chmIDF.IsChecked;
            settings.Save();             
        }

        /// <summary>
        /// Shows open file dialog for medical record file.
        /// Button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void btnInputFileDialog_Click(object sender, RoutedEventArgs e)
        {            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();
            if (result != true) return;            
            settings.ReportsFile = dlg.FileName;
            tbInputTrainFile.Text = settings.ReportsFile;
        }

        /// <summary>
        /// Shows open file dialog for ARFF file to be generated.
        /// Button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutputFileDialog_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = GenerateOutputFileName();
            dlg.DefaultExt = ".arff"; 
            dlg.Filter = "Text documents (.arff)|*.arff"; 
            
            Nullable<bool> result = dlg.ShowDialog();
            
            if (result == true)
            {
                settings.ARFFFile = dlg.FileName;
                tbOuputFile.Text = settings.ARFFFile; ;
            }
        }

        /// <summary>
        /// Generates output file name automaticly.
        /// </summary>
        /// <returns></returns>
        private string GenerateOutputFileName()
        {
            string name = tbDiagnoses.Text;
            if (chbBigrams.IsEnabled) name += "b" + tbBigramsCount.Text;
            if (chbTopWords.IsEnabled) name += "b" + tbTopWordsCount.Text;
            return name;
        }

        /// <summary>
        /// Validate program settings, save settings and begin generation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {            
            if (!Validate()) return;
            SaveSettings();
            BeginGeneration();
        }
        /// <summary>
        /// Start long operation generate ARFF file in separeted file in order to prevent
        /// all GUI to be frozen.
        /// </summary>
        private void BeginGeneration()
        {
            tbError.Text = "";
            btnGenerate.IsEnabled = false;
            progressBar.IsIndeterminate = true;
            Action startBuilding;
            startBuilding = () => DoBuilding();
            var t = new Thread(startBuilding.Invoke);
            t.Start();
        }

        /// <summary>
        /// Building ARFF file and when finished it stops progress bar and enables Generate  button 
        /// </summary>
        private void DoBuilding()
        {
            BuildAndWrite();
            progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate() { progressBar.IsIndeterminate = false; }));
            btnGenerate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate() { btnGenerate.IsEnabled = true; }));
        }

        /// <summary>
        /// Generates bitmap and ARFF file.
        /// </summary>
        private void BuildAndWrite()
        {
            IBitmap bitmap = new Bitmap(settings);
            bitmap.BuildBitmap();
            bitmap.WriteToARFFFile();
        }        

        /// <summary>
        /// Validates all GUI fields and existance of files.
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            if (tbInputTrainFile.Text == "")
            {
                ErrorMsg = "Input file is missing.";
                return false;
            }

            if (!File.Exists(tbInputTrainFile.Text))
            {
                ErrorMsg = "Input file does not exist.";
                return false;
            }

            if (chbFeaturesLoad.IsChecked.Value && settings.FeaturesFile != null && settings.FeaturesFile != "" && !File.Exists(settings.FeaturesFile))
            {
                ErrorMsg = "Features file does not exist.";
                return false;
            }

            if ((chbBigrams.IsEnabled && tbBigramsCount.Text == "") || (chbTopWords.IsEnabled && tbTopWordsCount.Text == ""))
            {
                ErrorMsg = "Please, fill in the count of features.";
                return false;
            }

            int number = 0;
            if ((chbBigrams.IsEnabled && !Int32.TryParse(tbBigramsCount.Text, out number)) || (chbTopWords.IsEnabled && !Int32.TryParse(tbTopWordsCount.Text, out number)))
            {
                ErrorMsg = "Count field must be an integer.";
                return false;
            }


            if ((chbBigrams.IsEnabled && tbBigramsFrequency.Text == "") || (chbTopWords.IsEnabled && tbTopWordsFrequency.Text == ""))
            {
                ErrorMsg = "Please, fill in the count of features.";
                return false;
            }

            if ((chbBigrams.IsEnabled && !Int32.TryParse(tbBigramsFrequency.Text, out number)) || (chbTopWords.IsEnabled && !Int32.TryParse(tbTopWordsFrequency.Text, out number)))
            {
                ErrorMsg = "Min. frequency field must be an integer.";
                return false;
            }

            if (tbDiagnoses.Text == "")
            {
                ErrorMsg = "Please, fill in a desired diagnosis.";
                return false;
            }

            if (chmStopWords.IsChecked && (settings.Preprocessing.StopwordsInput == null || !File.Exists(settings.Preprocessing.StopwordsInput)))
            {
                ErrorMsg = "Inputs -> Stop words has invalid file path.";
                return false;
            }

            if (tbOuputFile.Text == "")
            {
                ErrorMsg = "Please, determine output file.";
                return false;
            }

            try
            {
                using (File.Create(tbOuputFile.Text)) { };
            }
            catch (Exception)
            {
                ErrorMsg = "Output file could not be created";
                return false;
            }

            return true;
        }


        /// <summary>
        /// Clear error message.
        /// </summary>
        private void ClearErrorMsg()
        {
            ErrorMsg = "";
        }

        private string ErrorMsg
        {
            get { return tbError != null ? tbError.Text : null; ;}
            set { tbError.Text = value; }
        }

       /// <summary>
       /// Opens load dialog for stop words.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void MenuItemStopWords_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Stop words";
            dialog.FilePath = settings.Preprocessing.StopwordsInput;
            var result = dialog.ShowDialog();
            if (result == true) settings.Preprocessing.StopwordsInput = dialog.FilePath; 
        }

        /// <summary>
        /// Opens load dialog for features.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemFeatures_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Features";
            dialog.FilePath = settings.FeaturesFile;
            var result = dialog.ShowDialog();
            if (result == true) settings.FeaturesFile = dialog.FilePath;
            
        }

        /// <summary>
        /// Opens load dialog for morfology. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemMorphology_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Morphology";
            dialog.FilePath = settings.Preprocessing.MorphologyInput;
            if (dialog.ShowDialog() == true) settings.Preprocessing.MorphologyInput = dialog.FilePath;
        }        

        /// <summary>
        /// Closes application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Shows confirmations dialog to close program when the ARFF file is being benerated. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (progressBar.IsIndeterminate)
            {
                MessageBoxResult result = MessageBox.Show("Do you wish to cancel the creation of ARFF file?", "Quit confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) e.Cancel = true;
            }
        }

        /// <summary>
        /// Shows Guide Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuideMenutItem_Click(object sender, RoutedEventArgs e)
        {
            Guide guide = new Guide();
            guide.ShowDialog();
        }

        /// <summary>
        /// Shows About Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMenutItem_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }       

        /// <summary>
        /// Changes ARFF file name when diagnosis has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDiagnoses_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbOuputFile.Text == "") return;
            string d = System.IO.Path.GetDirectoryName(tbOuputFile.Text);
            tbOuputFile.Text = string.Format("{0}\\{1}.arff", d, GenerateOutputFileName());
        }
        
        
    }

    /// <summary>
    /// WPF negate converter
    /// </summary>
    public class NegateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)  return !(bool)value;            
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return !(bool)value;            
            return value;
        }

    }
}
