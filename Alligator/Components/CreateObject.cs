using System;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;
using System.Collections.Generic;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.Base
{
    public class CreateJson : GH_Component, IGH_VariableParameterComponent
    {
        public CreateJson() : base("CreateBHoMObject", "BHoMObj", "Creates a custom BHoMObject from custom inputs", "Alligator", "Base") { }
        public override Guid ComponentGuid { get { return new Guid("dbd3fe50-423a-4ea4-8bc7-7ad94d1d67e9"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public BH.oM.Base.CustomObject customObj { get; set; } = new oM.Base.CustomObject();

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }
        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Grasshopper.Kernel.Parameters.Param_GenericObject();
        }
        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }
        public void VariableParameterMaintenance()
        {
            for (int i = 2; i < Params.Input.Count; i++)
            {
                Params.Input[i].Access = GH_ParamAccess.list;
                Params.Input[i].NickName = "D" + (i-2);
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the object", GH_ParamAccess.item);
            pManager.AddTextParameter("Tags", "Tags", "Tags of the object", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoMObject", "BHoM", "BHoMObject", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            customObj = new oM.Base.CustomObject();
            string name = "";
            List<string> tags = new List<string>();
            name = DA.BH_GetData(0, name);
            tags = DA.BH_GetDataList(1, tags);
            customObj.Name = name;
            customObj.Tags = new HashSet<string>(tags);
            for (int i = 2; i < Params.Input.Count; i++) // Skips Name and Tags parameter
            {
                List<object> dX = new List<object>();
                dX = DA.BH_GetDataList(i, dX);
                customObj.CustomData.Add(Params.Input[i].NickName, dX);
            }
            DA.BH_SetData(0, customObj);
        }
    }
}