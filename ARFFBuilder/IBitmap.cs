using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder
{
    public interface IBitmap
    {
        void BuildBitmap();
        void WriteToARFFFile();
    }
}
