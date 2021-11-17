using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Grasshopper
{
    [Description("Settings for the Grasshopper UI.")]
    public class UISettings : BHoMObject, ISettings
    {
        public virtual bool UseWireMenu { get; set; } = true;
    }
}
