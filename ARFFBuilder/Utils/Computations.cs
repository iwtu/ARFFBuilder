using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARFFBuilder.Entity;

namespace ARFFBuilder.Utils
{
    /// <summary>
    /// Machine Learning and information retrieval patterns and compuations. 
    /// </summary>
    public static class Computations
    {        
       private static long cislo = 0; 
       /// <summary>
       /// Formula for pointwise mutual information. 
       /// Used in feature's selection.
       /// </summary>
       /// <param name="x">Attribte.</param>
       /// <param name="CountOfReports">Count of all reports</param>
       /// <param name="Dgf">Frequency of diagnosis.</param>
       /// <returns></returns>
        public static double  GetPMI(FAttribute x, int CountOfReports, int Dgf)
        {
            Computations.cislo++;
            var number = ((double)(x.DgFrequency * CountOfReports)) / (x.Frequency * Dgf);
            return Math.Log(number, 2);            
        }

        /// <summary>
        /// Formula for Invert Document Frequency.
        /// Used in stop words selection.
        /// </summary>
        /// <param name="x">Attribute</param>
        /// <param name="reportCount">Number of reports.</param>
        /// <returns></returns>
        public static double GetIDF(FAttribute x, int reportCount)
        {
            var result = Math.Log((double)reportCount / x.DgFrequency, 10);
            return result;
        }
    }
}
