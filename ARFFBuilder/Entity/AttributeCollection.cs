using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARFFBuilder.ReportPreprocessing;
using ARFFBuilder.Utils;

namespace ARFFBuilder.Entity
{
    /// <summary>
    /// Abstract collection for attributes and their properties needed for computations.
    /// Defines common behaviour for all attributes which does not depend on type of attributes
    /// </summary>
    public abstract class AttributeCollection : SettingsClass
    {
        /// <summary>
        /// Key is name of the attribute. Every attribute has uniq name.
        /// Value is attribute itself.
        /// </summary>
        protected readonly Dictionary<string, FAttribute> dicattributes = new Dictionary<string, FAttribute>();
        protected int DiagnosisCount = 0;
        protected int ReportsCount = 0;
        protected long SumAttributuesFrequency = 0;
        protected long SumAttributeDiagnosisFrequency = 0;
        protected readonly IPreprocessing preprocessing;

        public AttributeCollection(Settings settings) : base(settings) 
        {
            preprocessing = new Preprocessing(settings);
        }

        
        /// <summary>
        /// Adding attributes depending on type of the attributes.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="trainingDg"></param>
        protected void Add(string Name, string report, bool trainingDg)
        {
            if (dicattributes.ContainsKey(Name))
            {
                dicattributes[Name].Frequency += FAttribute.Occurences(Name, report);
                if (trainingDg) dicattributes[Name].DgFrequency++;
            }
            else
            {
                AddNew(Name, report, trainingDg);
            }          
        }

        /// <summary>
        /// Adding the new one attribute.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="mreport"></param>
        /// <param name="trainingDg"></param>
        protected abstract void AddNew(string Name, string report, bool trainingDg);


        /// <summary>
        /// Add all attributes from medical report.
        /// </summary>
        /// <param name="report"></param>
        public void Add(MedicalReport mreport)
        {
            string report = preprocessing.Preprocess(mreport.Report);
            foreach (var attributeName in ReportParse(report))
                Add(attributeName, report, settings.Diagnosis == mreport.Diagnosis);
            
            if (settings.Diagnosis == mreport.Diagnosis) DiagnosisCount++;
            ReportsCount++;
        }

        /// <summary>
        /// Builds all attributes;
        /// </summary>
        /// <param name="reports"></param>
        public void BuildAll(List<MedicalReport> reports)
        {
            foreach (var report in reports)
                Add(report);
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (!settings.PMI) return;

            int Xf = GetAttributesFrequency();
            foreach (var pair in dicattributes)
            {
                pair.Value.PMI = Computations.GetPMI(pair.Value, ReportsCount, DiagnosisCount);
            }
        }

        /// <summary>
        /// Parse medical report to attributes. It depends of type of feature.
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        protected abstract List<string> ReportParse(string report);

       /// <summary>
       /// Returns all attributes;
       /// </summary>
       /// <returns></returns>
        public List<FAttribute> GetAllAtributes()
        {
            return dicattributes.Values.ToList();
        }

        /// <summary>
        /// Sort attributes by frequency or PMI depending on settings objects.
        /// </summary>
        /// <param name="maxCount">Maximal count of attributes.</param>
        /// <param name="minFrequency">Consider only attributes with minimal frequency</param>
        /// <returns>Maximal counts of sorted attributes.</returns>
        public List<FAttribute> GetSortedBySettings(int maxCount, int minFrequency)
        {
            return dicattributes
                 .Where(b => b.Value.Frequency >= minFrequency)
                 .OrderByDescending(b => settings.PMI ? b.Value.PMI = Utils.Computations.GetPMI(b.Value, ReportsCount, DiagnosisCount  ): b.Value.Frequency)
                 .Take(maxCount)
                 .Select(b => b.Value) //.Select(new Bigram(b.Key, b.Value)
                 .ToList();
        }

        /// <summary>
        /// Get frequency of all attributes.
        /// </summary>
        /// <returns></returns>
        private int GetAttributesFrequency()
        {
            int f = 0;
            foreach (var x in dicattributes) f += x.Value.Frequency;
            return f;
        }
                

        private void ComputeFrequency()
        {
            int f = 0;
            int DgXF = 0;
            foreach (var x in dicattributes) {
                f += x.Value.Frequency;
                DgXF += x.Value.DgFrequency;
            }
            
        }

    }
}
