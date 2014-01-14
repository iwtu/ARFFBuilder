using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARFFBuilder.Entity;

namespace ARFFBuilder.ReportPreprocessing
{
    public interface IStopWords
    {
        bool IsStopWord(string word);
        List<FAttribute> GenerateByIDF(int maxCount);
    }
}
