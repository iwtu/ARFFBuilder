using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder.ReportPreprocessing
{
    public interface IStemmer
    {
        string Stem(string word);
    }
}
