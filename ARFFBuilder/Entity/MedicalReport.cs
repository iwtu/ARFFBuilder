using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder.Entity
{
    /// <summary>
    /// Medical Report consist of diagnoses and report.
    /// </summary>
    public class MedicalReport
    {
        public MedicalReport(string diagnosis, string report) 
        {
            this.Diagnosis = diagnosis;
            this.Report = report;
        }
        public string Diagnosis { get; set; }
        public string Report { get; set; }
    }
}
