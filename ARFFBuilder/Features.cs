using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using ARFFBuilder.Entity;


namespace ARFFBuilder
{
    /// <summary>
    /// Class for building and processing features.
    /// </summary>
    public class Features : SettingsClass, IFeatures
    {
        private List<MedicalReport> reports;        

        private UnigramCollection unigrams;
        private BigramCollection bigrams;

        public Features(Settings settings, List<MedicalReport> reports)
            : base(settings)
        {
            this.reports = reports;
            
        }

        /// <summary>
        /// Inits unigrams in case that the did loaded before and settings did not change. 
        /// In GUI usage they sometimes don't have to be loaded again.
        /// </summary>
        private void InitUnigrams()
        {            
            unigrams = new UnigramCollection(settings);
            unigrams.BuildAll(reports);
        } 

        /// <summary>
        /// Inits bigrams in case that the did loaded before and settings did not change. 
        /// In GUI usage they sometimes don't have to be loaded again.
        /// </summary>
        private void InitBigrams()
        {         
            bigrams = new BigramCollection(settings);
            bigrams.BuildAll(reports);
        }
        /// <summary>
        /// Build features depending on settings object.
        /// </summary>
        /// <returns></returns>
        public List<FAttribute> BuildFeatures()
        {
            if (settings.LoadFeatures) return LoadFeaturesFromARFF(settings.FeaturesFile);

            var all = new List<FAttribute>();

            if (settings.Features.ContainsKey(FeaturesEnum.Unigrams))
            {                
                InitUnigrams();
                var tps = settings.Features[FeaturesEnum.Unigrams];
                all.AddRange(unigrams.GetSortedBySettings(tps.MaxCount, tps.MinFrequency)); 
            }

            if (settings.Features.ContainsKey(FeaturesEnum.Bigrams))
            {
                InitBigrams();
                var bs = settings.Features[FeaturesEnum.Bigrams];                
                all.AddRange(bigrams.GetSortedBySettings(bs.MaxCount, bs.MinFrequency));
            }

            return all;
        }
        
        /// <summary>
        /// Builds all unigrams if necessary
        /// </summary>
        /// <returns></returns>
        public List<FAttribute> BuildAllUnigrams()
        {
            InitUnigrams();
            return unigrams.GetAllAtributes();
        }
        /// <summary>
        /// Loads attributes from ARFF file. Mainly for testing data set.
        /// For testing data set it has to be used exactly the same features including their order
        /// as for training data set. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<FAttribute> LoadFeaturesFromARFF(string fileName)
        {
            List<FAttribute> fattributes = new List<FAttribute>();

            using (StreamReader reader = File.OpenText(fileName))
            {
                string relation = "";
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith("@DATA"))
                        break;
                    if (line.StartsWith("@RELATION"))
                    {
                        var sa = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sa.Length != 2) continue;
                        relation = Utils.StringUtils.TrimQuotes(sa[1]);
                    }

                    var a = Regex.Match(line, ".*@ATTRIBUTE[ \t]+(.*)[ \t]+{");
                    if (!a.Success) continue;

                    string attributeName = Utils.StringUtils.TrimQuotes(a.Groups[1].Value);
                    if (attributeName == relation) continue;

                    switch (FAttribute.GetAttributeType(attributeName))
                    {
                        case FeaturesEnum.Bigrams:
                            fattributes.Add(new Bigram(attributeName));
                            break;
                        case FeaturesEnum.Unigrams:
                            fattributes.Add(new Unigram(attributeName));
                            break;
                    }
                }
            }

            return fattributes;
        }

    }
}
