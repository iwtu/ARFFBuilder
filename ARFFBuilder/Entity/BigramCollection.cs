using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder.Entity
{
    /// <summary>
    /// Bigram collection implements unique behaviour for bigrams.
    /// </summary>
    public class BigramCollection : AttributeCollection
    {

        public BigramCollection(Settings settings) : base(settings) { }
        protected override void AddNew(string Name, string report, bool trainingDg)
        {            
                dicattributes[Name] = new Bigram(Name, trainingDg, FAttribute.Occurences(Name, report));
        }

        protected override List<string> ReportParse(string report)
        {
            List<string> bigramStrings = new List<string>();
            var words = report.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for(int i = 0; i < words.Count - 1; i++)
            {
                bigramStrings.Add(words[i] + ' ' + words[i + 1]);
            }
            return bigramStrings;
        }
    
    }
}
