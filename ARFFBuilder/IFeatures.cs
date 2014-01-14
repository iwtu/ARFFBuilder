using ARFFBuilder.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARFFBuilder
{
    public interface IFeatures 
    {
        List<FAttribute> BuildFeatures();
        List<FAttribute> BuildAllUnigrams();

    }
}
