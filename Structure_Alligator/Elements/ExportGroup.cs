using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = BH.Engine.Grasshopper;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using BHB = BH.oM.Base;


namespace BH.UI.Alligator.Structural.Elements
{
    public class ExportGroup : GHE.Components.ExportComponent<BHB.BHoMGroup>
    {
        public ExportGroup() : base("Export Group", "SetGroup", "Creates or Replaces groups", "Structure", "Elements") { }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Group_Export; }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
      
        public override Guid ComponentGuid
        {
            get { return new Guid("E99F6CA7-A8F0-41DB-B3C5-E7C499B7A7C1"); }
        }

        protected override List<BHB.BHoMGroup> SetObjects(BHI.IElementAdapter app, List<BHB.BHoMGroup> objects, out List<string> ids)
        {
            app.SetGroups(objects, out ids);
            return objects;
        }
    }
}