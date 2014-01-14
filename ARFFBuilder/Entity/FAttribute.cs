using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ARFFBuilder.Entity
{
    /// <summary>
    /// Feature attribute
    /// </summary>
    public abstract class FAttribute
    {
        /// <summary>
        /// Usually same as its value.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Represents frequency of the attribute together
        /// </summary>
        public int Frequency { get; set; }       

        
        /// <summary>
        /// Frequency of the attribute and given diagnosis together
        /// </summary>
        public int DgFrequency { get;  set; }

        /// <summary>
        /// Pointwise mutual information
        /// </summary>
        public double PMI { get; set; }

        
        public FAttribute()
        {
            Frequency = 1;
        }
        
        public FAttribute(string name) : this()
        {
            Name = name;            
        }

        public FAttribute(string name, bool traingingDg) : this(name)
        {            
            DgFrequency = traingingDg ? 1 : 0;
        }

        public FAttribute(string name, bool traingingDg, int occurences) : this(name, traingingDg)
        {
            DgFrequency = traingingDg ? occurences : 0;
            Frequency = occurences;
        }

       
        
        public FAttribute(string name, int frequency, int dgFrequency) : this(name)
        {            
            Frequency = frequency;
            DgFrequency = dgFrequency;
        }

        /// <summary>
        /// Returns number of attribute occurences in medical report.
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public static int Occurences(string name, string report)
        {
            string word = Regex.Replace(name, @"\W", string.Empty);
            return Regex.Matches(report, @"\b" + Regex.Escape(word) + @"\b").Count;
        }

        /// <summary>
        /// Determines type of given feature attribute.
        /// </summary>
        /// <param name="fatrribute"></param>
        /// <returns></returns>
        public static FeaturesEnum GetAttributeType(string atrribute)
        {
            return atrribute.Contains(' ') ? FeaturesEnum.Bigrams : FeaturesEnum.Unigrams;
        }  
    }
}
