using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace Chrome_Alligator
{
    public class MapChart : GH_Component
    {
        public MapChart() : base("MapChart", "MapChart", "Define the config for a map chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("2A090E72-CC77-4AC2-B223-3E605E41C8F1");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("parent", "parent", "Define the parent to this component", GH_ParamAccess.item, "body");
            pManager.AddTextParameter("xDim", "xDim", "Dimension definition for the x axis", GH_ParamAccess.item);
            pManager.AddTextParameter("yDim", "yDim", "Dimension definition for the y axis", GH_ParamAccess.item);
            pManager.AddTextParameter("oDim", "oDim", "Dimension definition for the information display when the mouse is over an element.", GH_ParamAccess.item, "");
            pManager.AddNumberParameter("zoom", "zoom", "map zoom value. Default is 3", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("target", "target", "coordinates for the center of the map. Format is [latitude, longitude].", GH_ParamAccess.item, "");
            pManager.AddNumberParameter("radius", "radius", "point radius", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("style", "style", "Style of teh map", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double zoom = 0, radius = 0;
            string parent = "", xDim = "", yDim = "", oDim = "", target = "", style = "";
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref xDim);
            DA.GetData<string>(2, ref yDim);
            DA.GetData<string>(3, ref oDim);
            DA.GetData<double>(4, ref zoom);
            DA.GetData<string>(5, ref target);
            DA.GetData<double>(6, ref radius);
            DA.GetData<string>(7, ref style);

            List<string> config = new List<string>();

            config.Add("type: mapChart");
            config.Add("parent: " + parent);
            config.Add("xDim: " + xDim);
            config.Add("yDim: " + yDim);

            if (oDim.Length > 0)
                config.Add("oDim: " + oDim);

            if (zoom > 0)
                config.Add("zoom: " + zoom);

            if (target.Length > 0)
                config.Add("target: " + target);

            if (radius > 0)
                config.Add("radius: " + radius);

            DA.SetDataList(0, config);
        }
    }
}
