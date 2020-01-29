using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Grasshopper.Templates
{
    public interface IBHoMParam
    {
        Guid ComponentGuid { get; }

        string TypeName { get; }

        GH_Exposure Exposure { get; }

        bool Hidden { get; set; } 

        bool IsPreviewCapable { get; }

        Rhino.Geometry.BoundingBox ClippingBox { get; }

        Type ObjectType { get; set; }
    }
}
