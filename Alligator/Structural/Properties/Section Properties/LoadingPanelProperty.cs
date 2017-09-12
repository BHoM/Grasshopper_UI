using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHP = BH.oM.Structural.Properties;
using GHE = Grasshopper_Engine;

namespace Alligator.Structural.Properties.Section_Properties
{
    public class LoadingPanelProperty : GHE.Components.BHoMBaseComponent<BHP.LoadingPanelProperty>
    {
        public LoadingPanelProperty() : base("Create Loading panel Property", "LoadPanel", "Create a BH loading panel property object", "Structure", "Properties")
        {
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_Panel_Loading; }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the section Property", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Reference Edge", "RefEdge", "Reference edge for load application", GH_ParamAccess.item, 1);
            pManager.AddGenericParameter("CustomData", "CustomData", "CustomData", GH_ParamAccess.item);
            Params.Input[0].Optional = true;
            Params.Input[2].Optional = true;

            AppendEnumOptions("Load Application", typeof(BHP.LoadPanelSupportConditions));
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHP.LoadingPanelProperty loadPanelProp = new BHP.LoadingPanelProperty();

            loadPanelProp.LoadApplication = (BHP.LoadPanelSupportConditions)m_SelectedOption[0];

            int refEdge = 1;

            if(!DA.GetData(1, ref refEdge)) { return; }
            loadPanelProp.ReferenceEdge = refEdge;

            string name = GHE.DataUtils.GetData<string>(DA, 0);

            if (name != null)
                loadPanelProp.Name = name;

            Dictionary<string, object> customData = GHE.DataUtils.GetData<Dictionary<string, object>>(DA, 2);

            if (customData != null)
                loadPanelProp.CustomData = customData;

            DA.SetData(0, loadPanelProp);

        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("FF623BC9-1D75-4D74-B25A-3D9F3623428C");
            }
        }
    }
}
