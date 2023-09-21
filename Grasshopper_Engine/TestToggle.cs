using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Special;
using GH = Grasshopper;

namespace BH.Engine.Grasshopper
{
    public static partial class Modify
    {
        public static List<string> TestToggle()
        {
            var ghDoc = GH.Instances.ActiveCanvas.Document;
            var objs = ghDoc.Objects;

            List<string> guids = new List<string>();

            var toggleObjs = objs.Where(x => x.ComponentGuid.ToString() == "2e78987b-9dfb-42a2-8b76-3923ac8bd91a").ToList();

            foreach (var obj in toggleObjs)
            {
                var toggle = (GH.Kernel.Special.GH_BooleanToggle)obj;
                toggle.Value = false;
            }

            return guids;

            //GH.Instances.ActiveCanvas.
        }
    }
}
