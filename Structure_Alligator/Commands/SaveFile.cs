﻿using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using GHE = BH.Engine.Grasshopper;
using BHI = BH.oM.Structural.Interface;
using BHL = BH.oM.Structural.Loads;
using GH_IO.Serialization;
using BH.oM.Base;

namespace BH.UI.Alligator.Structural.Commands
{
    public class SaveFile : GH_Component
    {

        public SaveFile() : base("Save file", "Save", "Save file", "Structure", "Commands")
        {

        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("f4bc3b2f-ad1c-47ba-9e2d-2bbd72503795");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_App_Save; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to run", GH_ParamAccess.item);
            pManager.AddTextParameter("Filename", "F", "GSA Filename", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Activate", "Go", "Activate", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Returns wether the analysis was successfully run or not", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool success = false;
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.ICommandAdapter adapter = GHE.DataUtils.GetGenericData<BHI.ICommandAdapter>(DA, 0);
                string fileName = GHE.DataUtils.GetData<string>(DA, 1);

                success = adapter.Save(fileName);
            }

            DA.SetData(0, success);
        }
    }
}