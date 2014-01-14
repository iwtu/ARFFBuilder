using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ARFFBuilder.Entity
{
    /// <summary>
    /// Type of attribute.
    /// </summary>
    public class Bigram : FAttribute
    {
        public Bigram() : base() { }
        public Bigram(string Name) :base(Name) { }
        public Bigram(string name, bool trainingDg) : base(name, trainingDg) { }
        public Bigram(string name, bool traingingDg, int occurences) : base(name, traingingDg, occurences) { }
        public Bigram(string name, int frequency, int dgFrequency = 0) : base(name, frequency, dgFrequency) { }        
    }
}
