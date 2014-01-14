using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ARFFBuilder.Entity;
using System.IO;

namespace ARFFBuilder
{
    /// <summary>
    /// Class for loading and processing medical reports from file.
    /// </summary>
    public class Reports
    {
        /// <summary>
        /// Loaded only if new file name was selected
        /// </summary>
        private static List<MedicalReport> medicalReports;
        private static string inputRecordsFile;

        public static List<MedicalReport> MedicalReports { get { return medicalReports; } }        
        
        public Reports(string inputRecords)
        {
            if (inputRecordsFile != inputRecords)
            {
                medicalReports = ReadAllRecords(inputRecords);
                inputRecordsFile = inputRecords;
            }
        }       

        /// <summary>
        /// Reads records line by line and creates medical reports. 
        /// First word on line represents the diagnosis and rest of the line medical report.
        /// </summary>
        /// <param name="fileName">Input file for medical records.</param>
        /// <returns>List of created medical reports</returns>
        private List<MedicalReport> ReadAllRecords(string fileName) 
        {            
            List<MedicalReport> reports = new List<MedicalReport>(); 
            string line = "";
            
            using (StreamReader reader = File.OpenText(fileName))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] mreport = line.Split(new char[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (mreport.Length < 2) continue;
                    reports.Add(new MedicalReport(mreport[0], mreport[1]));                    
                }
            }

            return reports;
        }
    }
}
