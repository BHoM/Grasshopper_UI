using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using GHE = Grasshopper_Engine;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using GH_IO.Serialization;

namespace Alligator.Structural.Elements
{
    public class ExportBarDesignElement : GHE.Components.ExportComponent<BHE.Bar>
    {
        public ExportBarDesignElement() : base("Export Design Bar", "SetDesignBar", "Creates a bar design element to be used for codechecks", "Structure", "Element Design")
        { }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IDesignMemberAdapter app = GHE.DataUtils.GetGenericData<BHI.IDesignMemberAdapter>(DA, 0);
                if (app != null)
                {

                    List<BHE.Bar> clonedObjects = CloneObjects(GHE.DataUtils.GetGenericDataList<BHE.Bar>(DA, 1));

                    m_ids = null;

                    if (m_exportedObjects == null)
                        return;
                    app.SetBarDesignElement(clonedObjects, out m_ids);
                    m_exportedObjects = clonedObjects;
                }
            }

            DA.SetDataList(0, m_ids);
            DA.SetDataList(1, m_exportedObjects);
        }

        protected override List<BHE.Bar> SetObjects(BHI.IElementAdapter app, List<BHE.Bar> objects, out List<string> ids)
        {
            ids = null;
            return null;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("7C31AD6F-E7D6-4296-9DD5-EF9933E6B5C0"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_DesignBar_Export; }
        }

    }
}