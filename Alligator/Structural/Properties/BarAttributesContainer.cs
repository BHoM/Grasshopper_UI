using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSP = BH.oM.Structural.Properties;
using BHSE = BH.oM.Structural.Elements;

namespace Alligator.Structural.Properties
{

    /// <summary>
    /// Container of different bar attributes to pass between CreateAttributesComponent and CreateBarComponent
    /// </summary>
    public class BarAttributesContainer
    {
        public BHSP.BarRelease BarReleases { get; set; }
        public BHSE.BarFEAType FEAType { get; set; }
        public BHSE.BarStructuralUsage StructuralUsage { get; set; }

    }
}
