using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder.Entity
{
    /// <summary>
    /// Type of attribute.
    /// </summary>
    public class Unigram : FAttribute
    {
        public Unigram() : base() { }
        public Unigram(string Name) :base(Name) { }
        public Unigram(string name, bool trainingDg) : base(name, trainingDg) { }
        public Unigram(string name, bool traingingDg, int occurences) : base(name, traingingDg, occurences) { }
        public Unigram(string name, int frequency, int dgFrequency = 0) : base(name, frequency, dgFrequency) { }          
    }
}
