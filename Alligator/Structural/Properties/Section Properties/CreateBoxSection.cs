using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHP = BHoM.Structural.Properties;

namespace Alligator.Structural.Properties
{
    public class CreateBoxSection : GH_Component
    {
        public CreateBoxSection() : base("Custom Section Property", "SecProp", "Creates a custom section property from curves", "Structure", "Properties") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("42F18D98-FB51-4EFC-AE97-E680B43B433B");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Height", "H", "Height of the cross section", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "W", "Width of the cross section", GH_ParamAccess.item);
            pManager.AddNumberParameter("Flange Thickness", "TF", "Thickness of the flange", GH_ParamAccess.item);
            pManager.AddNumberParameter("Web Thickness", "TW", "Thickness of the web", GH_ParamAccess.item);

            pManager.AddNumberParameter("Outer radius", "OR", "Outer radius, optional", GH_ParamAccess.item);
            pManager.AddNumberParameter("Inner radius", "IR", "Inner radius, optional", GH_ParamAccess.item);

            pManager[4].Optional = true;
            pManager[5].Optional = true;

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Box Section", "P", "The creates box section property", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double h, w, tf, tw, or, ir;
            h = w = tf = tw = or = ir = 0;

            if (!DA.GetData(0, ref h)) { return; }
            if (!DA.GetData(1, ref w)) { return; }
            if (!DA.GetData(2, ref tf)) { return; }
            if (!DA.GetData(3, ref tw)) { return; }

            BHP.SectionProperty prop;

            if (DA.GetData(4, ref or) && DA.GetData(5, ref ir))
            {
                prop = BHP.SectionProperty.CreateBoxSection(h, w, tf, tw, or, ir);
            }
            else
            {
                prop = BHP.SectionProperty.CreateBoxSection(h, w, tf, tw);
            }

            DA.SetData(0, prop);
                
        }
    }
}
