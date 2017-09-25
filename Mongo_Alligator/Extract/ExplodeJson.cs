using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Collections;
using System.Windows.Forms;
using BH.UI.Alligator.Base;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using BH.oM.Base;
using BH.Engine.Reflection;

namespace BH.UI.Alligator.Mongo
{
    public class ExplodeJson : GH_Component, IGH_VariableParameterComponent
    {
        public ExplodeJson() : base("ExplodeJson", "ExplodeJson", "Explode json string into objects", "Alligator", "Mongo") { }
        public override Guid ComponentGuid { get { return new Guid("020CF2C8-CB67-4731-9CCA-50F0932E18DC"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Resources.BHoM_Mongo_FromJson; } }
        private Dictionary<string, object> m_Outputs = new Dictionary<string, object>();

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
            pManager.AddTextParameter("json", "json", "json object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string json = "";
            json = DA.BH_GetData(0, json);
            if (json == null) return;

            Dictionary<string, object> bJson = BsonDocument.Parse(json).ToDictionary();
            //CustomObject customObj = obj.ToDictionary() as CustomObject; // TODO Implement explicit cast between Dictionary and Custom Object
            m_Outputs = bJson["CustomData"] as Dictionary<string, object>;

            List<string> keys = m_Outputs.Keys.ToList();
            if (keys.Count == Params.Output.Count)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    var val = m_Outputs[keys[i]];
                    if (keys[i] == "_t") { Convert.ChangeType(m_Outputs[keys[i]], Type.GetType(); }
                    Convert.ChangeType(m_Outputs[keys[i]], Type.GetType();
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

        protected override void AfterSolveInstance()
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
        }

    }
}
