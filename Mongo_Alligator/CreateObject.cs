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
    public class CreateObject : GH_Component, IGH_VariableParameterComponent
    {
        public CreateObject() : base("CreateObject", "CreateObject", "Create object from inputs", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("EE290FE3-A598-4DC3-9069-B494C94CA662");
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
            pManager.AddGenericParameter("object", "object", "object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            for (int i = 0; i < Params.Input.Count; i++)
            {
                IGH_Param param = Params.Input[i];
                string key = param.NickName;
                List<object> objects = new List<object>();
                DA.GetDataList<object>(i, objects);

                switch (objects.Count())
                {
                    case 0:
                        continue;
                    case 1:
                        dic[key] = GHE.DataUtils.UnwrapObject(objects[0]);
                        break;
                    default:
                        dic[key] = objects.Select(x => GHE.DataUtils.UnwrapObject(x)).ToList();
                        break;
                }
            }

            DA.SetData(0, dic);
        }
    }
}
