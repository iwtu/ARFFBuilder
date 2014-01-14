using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder.Entity
{
    /// <summary>
    /// Program input settings base on which will be generated ARFF file. 
    /// </summary>
    public class Settings
    {
        public string ReportsFile;
        public string ARFFFile;
        public string Diagnosis;
        public string FeaturesFile;        
        public bool LoadFeatures;
        public bool PMI;



        public PreprocessingUsage Preprocessing = new PreprocessingUsage();
        //key si FeatureEnum
        public Dictionary<FeaturesEnum, Feature> Features = new Dictionary<FeaturesEnum, Feature>();
        
        private bool reportsHasChanged = true;
        public bool ReportsHasChanged { get { return reportsHasChanged; } }
        private bool stopwordsHasChanged = true;
        public bool StopwordsHasChanged { get { return stopwordsHasChanged; } }

        private bool morfologyHasChanged = true;
        public bool MorfologyHasChanged { get { return morfologyHasChanged; } }

        public Settings()
        {
            CheckChanges(null);
        }

        /// <summary>
        /// Sets HasChanged property.
        /// </summary>
        /// <param name="other"></param>
        public void CheckChanges(Settings other)
        {
            if (other == null) return;
            
            reportsHasChanged = !(this.ReportsFile != other.ReportsFile);
            stopwordsHasChanged = !(this.Preprocessing.StopwordsInput != other.Preprocessing.StopwordsInput);
            morfologyHasChanged = !(this.Preprocessing.MorphologyInput != other.Preprocessing.MorphologyInput);
        }

        /// <summary>
        /// Make copy of all settings.
        /// </summary>
        /// <returns>Copied Settings</returns>
        public Settings CopySettings()
        {
            var s = new Settings();
            s.ReportsFile = this.ReportsFile;
            s.LoadFeatures = this.LoadFeatures;
            s.FeaturesFile = this.FeaturesFile;            
            s.Diagnosis = this.Diagnosis;
            s.ARFFFile = this.ARFFFile;            
            s.Preprocessing.UseCzechStemmer = this.Preprocessing.UseCzechStemmer;
            s.Preprocessing.FilterStopWords = this.Preprocessing.FilterStopWords;
            s.Preprocessing.IgnorePunctuation = this.Preprocessing.IgnorePunctuation;
            s.Preprocessing.StopwordsInput = this.Preprocessing.StopwordsInput;
            s.Features = new Dictionary<FeaturesEnum, Feature>();
            foreach (var feature in Features)
            {
                s.Features[feature.Key] = new Feature(feature.Value.Name, feature.Value.MaxCount, feature.Value.MinFrequency);
            }


            s.LoadFeatures = this.LoadFeatures;
            s.PMI = this.PMI;
            s.Preprocessing.StopwordsIDF = this.Preprocessing.StopwordsIDF;
            s.Preprocessing.StopWordsIDFCount = this.Preprocessing.StopWordsIDFCount;
            s.Preprocessing.StopWordsIDFOutput = this.Preprocessing.StopWordsIDFOutput;
            s.Preprocessing.MorphologyInput = this.Preprocessing.MorphologyInput;

            return s;
        }

        /// <summary>
        /// Save all settings to user account of the applications;
        /// </summary>
        public void Save()
        {
            var ds = Properties.Settings.Default;
            ds.InputTrainFile = ReportsFile;
            ds.Diagnoses = Diagnosis;
            ds.UseStemmer = Preprocessing.UseCzechStemmer;
            ds.IgnorePunctuation = Preprocessing.IgnorePunctuation;
            ds.IgnoreStopWords = Preprocessing.FilterStopWords;

            ds.Bigrams = Features.ContainsKey(FeaturesEnum.Bigrams);
            ds.TopWords = Features.ContainsKey(FeaturesEnum.Unigrams);
            if (Features.ContainsKey(FeaturesEnum.Unigrams))
            {
                ds.TopWordsCount = Features[FeaturesEnum.Unigrams].MaxCount.ToString();
                ds.TopWordsFrequency = Features[FeaturesEnum.Unigrams].MinFrequency.ToString();
            }
            if (Features.ContainsKey(FeaturesEnum.Bigrams))
            {
                ds.BigramsCount = Features[FeaturesEnum.Bigrams].MaxCount.ToString();
                ds.BigramsFrequency = Features[FeaturesEnum.Bigrams].MinFrequency.ToString();
            }

            ds.OutputFile = ARFFFile;
            ds.StopWordsFile = Preprocessing.StopwordsInput;
            ds.LoadFeature = LoadFeatures;
            ds.FeaturesFile = FeaturesFile;
            ds.Save();
        }

        /// <summary>
        /// Class for Preprocessing settings.
        /// </summary>
        public class PreprocessingUsage
        {
            public bool UseCzechStemmer;
            public bool FilterStopWords;
            public bool IgnorePunctuation;            
            public string StopwordsInput;            
            public bool StopwordsIDF;
            public int StopWordsIDFCount;
            public string StopWordsIDFOutput;
            public string MorphologyInput;            

            //public PreprocessingUsage(bool useStemmer, bool removeStopWords, bool removeInterpunction)
            //{
            //    UseCzechStemmer = useStemmer;
            //    FilterStopWords = removeStopWords;
            //    IgnorePunctuation = removeInterpunction;
            //}

            /// <summary>
            /// String of used settings;
            /// </summary>
            /// <returns>Return string describing which settings are used.</returns>
            public string UsageDescription()
            {
                string d = UseCzechStemmer ? "stemmering, " : "";
                d += FilterStopWords ? "without stop words," : "";
                d += IgnorePunctuation ? "without interpunction" : "";
                return d;
            }
        }

        /// <summary>
        /// General class for Feature settings. It can be different for different types of attributes.
        /// </summary>
        public class Feature
        {
            public FeaturesEnum Name { get; set; }
            public int MaxCount { get; set; }
            public int MinFrequency { get; set; }


            public Feature(FeaturesEnum name, int maxCount, int minFrequency)
            {
                Name = name;
                MaxCount = maxCount;
                MinFrequency = minFrequency;
            }

            public string Describe()
            {
                return string.Format("{0} MaxCount: {1} MinFrequency: {2}", Name, MaxCount, MinFrequency);
            }
        }

        /// <summary>
        /// Desribes all selected features.
        /// </summary>
        /// <returns></returns>
        public string FeaturesDescription()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var feature in Features)
            {
                sb.Append(feature.Value.Describe());
                sb.Append(", ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns which filter is used if any.
        /// </summary>
        /// <returns></returns>
        public string FilterDescription()
        {
            string filter = PMI ? "PMI, " : "";
            return filter += Preprocessing.StopwordsIDF ? "IDF" : "";
        }
       
    }
}
