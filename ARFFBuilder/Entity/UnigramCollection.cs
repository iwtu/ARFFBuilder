using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder.Entity
{
    /// <summary>
    /// Bigram collection implements unique behaviour for bigrams.
    /// </summary>
    public class UnigramCollection : AttributeCollection
    {

        public UnigramCollection(Settings settings) : base(settings) { }
        protected override void AddNew(string Name, string report, bool trainingDg)
        {                       
             dicattributes[Name] = new Unigram(Name, trainingDg, FAttribute.Occurences(Name, report));
        }

        protected override List<string> ReportParse(string report)
        {
            return report.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
