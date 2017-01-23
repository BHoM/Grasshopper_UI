using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using MA = Mongo_Adapter;
using BHB = BHoM.Base;
using GHE = Grasshopper_Engine;

namespace Alligator.Mongo
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
            string json = "{";

            for (int i = 0; i < Params.Input.Count; i++)
            {
                IGH_Param param = Params.Input[i];
                string key = param.NickName;
                List<object> objects = new List<object>();
                DA.GetDataList<object>(i, objects);

                string valJson = "";
                switch (objects.Count())
                {
                    case 0:
                        continue;
                    case 1:
                        valJson = BHB.JSONWriter.Write(GHE.DataUtils.UnwrapObject(objects[0]));
                        break;
                    default:
                        valJson = "[" + objects.Select(x => BHB.JSONWriter.Write(GHE.DataUtils.UnwrapObject(x))).Aggregate((a, b) => a + ',' + b) + "]";
                        break;
                }

                json += "\"" + key + "\":" +valJson + ","; 
            }
            json = json.TrimEnd(',') + "}";

            DA.SetData(0, json);
        }
    }
}
