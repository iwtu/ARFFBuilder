using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARFFBuilder;
using ARFFBuilder.Entity;

namespace ARFFBuilder.ReportPreprocessing
{
    public class Preprocessing : SettingsClass, IPreprocessing
    {
        private StringBuilder sbuilder = new StringBuilder();

        private IStopWords stopwords;
        private IMorphology morphology;
        private IStemmer stemmer = new CzechStemmer();
        
        public Preprocessing(Settings settings) : base(settings) 
        { 
            morphology = new Morphology(settings);
            stopwords = new StopWords(settings);
        }
        
        public string Preprocess(string report)
        {
            sbuilder.Clear();
            string[] words = report.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            string w = "";

            foreach (string word in words)
            {
                w = word;
                w = morphology.GetLemma(w);
                w = w.ToLower();
                if (stopwords.IsStopWord(w)) continue;
                if (settings.Preprocessing.IgnorePunctuation) w = Utils.StringUtils.RemovePunctuation(w);
                if (w == string.Empty) continue;
                if (settings.Preprocessing.UseCzechStemmer) w = stemmer.Stem(w);

                sbuilder.Append(w);
                sbuilder.Append(' ');
            }
            return sbuilder.ToString();
        }
    }
}
