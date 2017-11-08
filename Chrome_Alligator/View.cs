﻿using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Chrome.Views;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;
using BH.oM.Base;

namespace BH.UI.Alligator.Chrome
{
    public class View : CreateObjectTemplate
    {
        public View() : base("ChromeView", "View", "Creates a specific view for data sent to Chrome", "Alligator", "Chrome") { }
        public override Guid ComponentGuid { get { return new Guid("1D91BF89-3111-40B0-A1F5-19460941493F"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.primary;  } }

        /*************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "View", "View", "View", GH_ParamAccess.item);
        }

        /*************************************/

        protected override void SetData(IGH_DataAccess DA, object result)
        {
            
            DA.SetData(0, new CustomObject(new Dictionary<string, object> { { "View", result } }));
        }

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type viewType = typeof(IView);
            return BH.Engine.Reflection.Create.TypeList().Where(x => viewType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }
    }
}