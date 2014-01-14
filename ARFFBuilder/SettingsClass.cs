using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARFFBuilder.Entity;

namespace ARFFBuilder
{
    /// <summary>
    /// An abstract class for Settings object which provides the Settings object 
    /// and define base constructor for all its descants. 
    /// </summary>
    public abstract class SettingsClass
    {
        protected readonly Settings settings;
        public SettingsClass(Settings settings)
        {
            this.settings = settings;
        }
    }
}
