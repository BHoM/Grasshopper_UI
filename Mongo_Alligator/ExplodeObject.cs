using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using MA = BH.Adapter.Mongo;
using BHB = BH.oM.Base;
using GHE = BH.Engine.Grasshopper;
using System.Collections;
using System.Windows.Forms;

namespace Alligator.Mongo
{
    public class ExplodeObject : GH_Component, IGH_VariableParameterComponent
    {
        public ExplodeObject() : base("ExplodeObject", "ExplodeObject", "Explode object into its parts", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("99EA41FB-B8E0-4E44-A76D-B0BA0AB07DEC");
            }
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
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

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("object", "object", "object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var input = GHE.DataUtils.GetData<object>(DA, 0);
            m_Outputs = input as Dictionary<string, object>;

            List<string> keys = m_Outputs.Keys.ToList();
            if (keys.Count == Params.Output.Count)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    var val = m_Outputs[keys[i]];
                    if (val is IList)
                        DA.SetDataList(i, val as IList);
                    else
                        DA.SetData(i, val);
                }
            }
            else
            {
                throw new Exception("The outputs need to be updated first. Please right-click on component and select update.");
            }
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Update Outputs", Menu_DoClick);
        }

        private void Menu_DoClick(object sender, EventArgs e)
        {
            List<string> keys = m_Outputs.Keys.ToList();

            int nbNew = keys.Count();
            int nbOld = Params.Output.Count();

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
            {
                Params.Output[i].NickName = keys[i];
            }

            for (int i = nbOld - 1; i > nbNew; i--)
                Params.UnregisterOutputParameter(Params.Output[i]);

            for (int i = nbOld; i < nbNew; i++)
            {
                Grasshopper.Kernel.Parameters.Param_GenericObject newParam = new Grasshopper.Kernel.Parameters.Param_GenericObject();
                newParam.NickName = keys[i];
                Params.RegisterOutputParam(newParam);
            }

            this.OnAttributesChanged();
            ExpireSolution(true);
        }

        private Dictionary<string, object> m_Outputs = new Dictionary<string, object>();
    }
}
