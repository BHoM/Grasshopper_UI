using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Newtonsoft.Json;
using BH.UI.Alligator.Base;
using MongoDB.Bson;

namespace BH.UI.Alligator.Mongo
{
    public class CreateJson : GH_Component, IGH_VariableParameterComponent
    {
        public CreateJson() : base("CreateJson", "CreateJson", "Create json object from inputs", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("A046C2E8-37C8-4AD3-A8A9-1D8849C53E8A");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Resources.BHoM_Mongo_ToJson; }
        }

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
                Params.Input[i].Access = GH_ParamAccess.item;
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
                customObj.Name = this.Name;
                object obj = new object();
                obj = DA.BH_GetData(i, obj);
                customObj.CustomData.Add(Params.Input[i].NickName, obj.UnwrapObject());
            }
            string json = customObj.ToJson();
            DA.SetData(0, json);
        }
        //public override string ToString()
        //{
        //    return Params.Output[0].ToString();
        //}
    }
}
