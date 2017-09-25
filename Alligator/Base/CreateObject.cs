using System;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Mongo
{
    public class CreateJson : GH_Component, IGH_VariableParameterComponent
    {
        public CreateJson() : base("CreateJson", "CreateJson", "Create json object from inputs", "Alligator", "Mongo") { }
        public override Guid ComponentGuid { get { return new Guid("dbd3fe50-423a-4ea4-8bc7-7ad94d1d67e9"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

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
            for (int i = 0; i < Params.Input.Count; i++)
            {
                Params.Input[i].Access = GH_ParamAccess.list;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("json", "json", "json object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.oM.Base.CustomObject customObj = new oM.Base.CustomObject();
            for (int i = 0; i < Params.Input.Count; i++)
            {
                object obj = Params.Input[i].UnwrapObject();
                customObj.CustomData.Add(Params.Input[i].NickName, obj);
                customObj.CustomData["BHoM_Guid"] = customObj.CustomData["BHoM_Guid"].ToString();
            }
            DA.SetData(0, customObj);
        }
    }
}