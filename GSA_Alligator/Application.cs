﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.Adapter.GSA;

namespace BH.UI.Alligator.GSA
{
    public class GSAApp : GH_Component
    {
        public GSAApp() : base("GSA Application", "GSAApp", "Creates a GSA Application", "Structure", "Application") { }
        public override Guid ComponentGuid { get { return new Guid("92ee004b-afe3-4ed3-b2d2-73248f8578d6"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_GSA_App; } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Filename", "F", "GSA Filename", GH_ParamAccess.item);
            pManager.AddGenericParameter("Settings", "S", "Application Settings", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "A", "GSA Application", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = "";
            GSAAdapter app;
            if (DA.GetData(0, ref filePath))
                app = new GSAAdapter(filePath);
            else
                app = new GSAAdapter();
            DA.SetData(0, app);
        }
    }
}