using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARFFBuilder.Entity;
using System.IO;

namespace ARFFBuilder
{
    /// <summary>
    /// Builds ARFF file for data set and selected features. 
    /// ARFF file containts bitmap of features. Medical report per line.  
    /// </summary>
    public class Bitmap : SettingsClass, IBitmap
    {

        Dictionary<string, List<bool>> bitmap;        
        
        private static List<MedicalReport> medicalReports;
        
        private static List<FAttribute> fattribues;

        private readonly IFeatures features;

        /// <summary>
        /// Reads medical report and builds features depeding on settings object.
        /// </summary>
        /// <param name="settings"></param>
        public Bitmap (Settings settings) : base(settings)
        {
            bitmap = new Dictionary<string, List<bool>>();       
            var reports = new Reports(settings.ReportsFile);
            
            medicalReports = Reports.MedicalReports;
            features = new Features(settings, medicalReports);
            fattribues = features.BuildFeatures();
            
        }
        
        /// <summary>
        /// Builds bitmap of features for medical reports.
        /// </summary>
        public void BuildBitmap()
        {

            foreach (FAttribute fattribute in fattribues) bitmap[fattribute.Name] = new List<bool>();              
                        
            bitmap[settings.Diagnosis] = new List<bool>();            

            foreach (MedicalReport mreport in medicalReports)
            {
                foreach (FAttribute fattribute in fattribues)
                    bitmap[fattribute.Name].Add(FAttribute.Occurences(fattribute.Name, mreport.Report) > 0 ? true : false);
                bitmap[settings.Diagnosis].Add(mreport.Diagnosis == settings.Diagnosis);
            }            
        }

        public void WriteToARFFFile()
        {
            WriteToARFFFile(settings.ARFFFile, bitmap);
        }

        /// <summary>
        /// Create ARFF file from bitmap. 
        /// </summary>
        /// <param name="fileName">Generated ARFF file.</param>
        /// <param name="bitmap">Bitmap of features for medical reports.</param>
        private void WriteToARFFFile(string fileName, Dictionary<string, List<bool>> bitmap)
        {
            if (File.Exists(settings.ARFFFile)) File.Delete(fileName);
            using (StreamWriter writer = File.CreateText(fileName))
            {
                writer.WriteLine(GetARFFHeader(fileName));

                //atributes
                writer.WriteLine("@RELATION " + settings.Diagnosis);
                writer.WriteLine();


                var sbline = new StringBuilder();
                foreach (var pair in bitmap)
                    if (pair.Key != settings.Diagnosis)
                    {
                        string attr = pair.Key.Replace("\"", "\\\"");
                        writer.WriteLine(string.Format("@ATTRIBUTE \"{0}\" {{0,1}}", attr));
                    }

                writer.WriteLine(string.Format("@ATTRIBUTE {0} {{no,yes}}", settings.Diagnosis));
                writer.WriteLine();
                writer.WriteLine("@DATA");


                for (int i = 0; i < bitmap.Values.First().Count; i++)
                {
                    sbline.Clear();
                    foreach (var pair in bitmap)
                        if (pair.Key != settings.Diagnosis)
                        {
                            sbline.Append(Convert.ToInt32(pair.Value[i]));
                            sbline.Append(',');
                        }
                    sbline.Append(bitmap[settings.Diagnosis][i] ? "yes" : "no");
                    //sbline.Append(bitmap[settings.Diagnosis][i] ? settings.Diagnosis : "non" + settings.Diagnosis);
                    writer.WriteLine(sbline);
                }
            }
        }

        /// <summary>
        /// Create commented header for ARFF file.
        /// </summary>
        /// <param name="fileName">Filename of generated ARFF file.</param>
        /// <returns>Header for ARRF file.</returns>
        private string GetARFFHeader(string fileName)
        {
            return string.Format(@"% This is a part of the bachelor thesis in Institute of Formal and Applied Linguistics
            % Faculty of Mathematics and Physics, Charles University in Prague, Czech Republic.
            %
            % Title of the thesis: Automatic assignment of diagnosis to medical reports
            % Author: Adrián Lachata
            % Supervisor: Jiří Hana, Ph.D.
            %
            %
            % Input File: {0} 
            % Diagnosis: {1} 
            % Used data preprocessing: {2} 
            % Filter: {3}
            % Features: {4}
            %
            % Date: {5} 
            %
            %", fileName, settings.Diagnosis, settings.Preprocessing.UsageDescription(), settings.FilterDescription(), settings.FeaturesDescription(), DateTime.Now);
        }
    }
}
