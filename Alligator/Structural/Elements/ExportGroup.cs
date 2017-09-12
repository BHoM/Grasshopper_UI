using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHB = BHoM.Base;


namespace Alligator.Structural.Elements
{
    public class ExportGroup : GHE.Components.ExportComponent<BHB.IGroup>
    {
        public ExportGroup() : base("Export Group", "SetGroup", "Creates or Replaces groups", "Structure", "Elements") { }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Group_Export; }
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

        protected override List<BHB.IGroup> SetObjects(BHI.IElementAdapter app, List<BHB.IGroup> objects, out List<string> ids)
        {
            app.SetGroups(objects, out ids);
            return objects;
        }
    }
}