using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Collections;
using System.Windows.Forms;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.Engine.Reflection;
using BH.oM.Geometry;
using BH.UI.Alligator;
using Grasshopper.Kernel.Parameters;
using BH.Adapter.Rhinoceros;
using System.Reflection;

namespace BH.UI.Alligator.Base
{
    public class ExplodeJson : GH_Component, IGH_VariableParameterComponent
    {
        public ExplodeJson() : base("ExplodeObject", "ExplodeObj", "Explode a BHoMObject into child objects", "Alligator", "Base") { }
        public override Guid ComponentGuid { get { return new Guid("f2080175-a812-4dfb-86de-ae7dc8245668"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public bool additional { get; set; }

        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }

        public bool CanInsertParameter(GH_ParameterSide side, int index) { return false; }
        public bool CanRemoveParameter(GH_ParameterSide side, int index) { return false; }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index) { return new Grasshopper.Kernel.Parameters.Param_GenericObject(); }
        public bool DestroyParameter(GH_ParameterSide side, int index) { return true; }
        public void VariableParameterMaintenance() { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(),"BHoMObject", "BHoM", "BHoMObject", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject bhObj = new BHoMObject();
            if (!DA.GetData(0, ref bhObj) || bhObj == null)
                return;

            Dictionary<string, object> outputs = bhObj.GetPropertyDictionary();
            m_OutputTypes = bhObj.GetType().GetProperties().Where(x => x.CanRead && x.CanWrite).ToList();

            List<string> keys = outputs.Keys.ToList();
            if (keys.Count == Params.Output.Count)
            {
                foreach (string key in keys)
                {
                    int i = -1;
                    for (int j = 0; j < Params.Output.Count; j++)
                    {
                        if (Params.Output[j].NickName == key)
                            i = j;
                    }
                    if (i < 0)
                        continue;

                    var val = outputs[key];
                    if (val == null)
                    {
                        DA.SetData(i, null);
                        continue;
                    }
                        
                    var type = val.GetType();
                    if (typeof(IEnumerable).IsAssignableFrom(val.GetType()) && type != typeof(string) && !typeof(IDictionary).IsAssignableFrom(type))
                    {
                        if (Params.Output[i] is Param_Geometry)
                            DA.SetDataList(i, ((IEnumerable)val).Cast<IBHoMGeometry>().Select(x => x.IToRhino()).ToList());
                        else
                            DA.SetDataList(i, ((IEnumerable)val).Cast<object>().ToList());
                    }
                    else
                    {
                        if (val is IBHoMGeometry)
                            DA.SetData(i, ((IBHoMGeometry)val).IToRhino());
                        else
                            DA.SetData(i, val);
                    }
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "The outputs need to be updated first. Please right-click on component and select update.");
            }
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Update Outputs", Menu_DoClick);
            Menu_AppendItem(menu, "Append additional data", Menu_SetTrue);
        }
        private void Menu_SetTrue(object sender, EventArgs e)
        {
            if (additional) { additional = false; }
            else { additional = false; }
            UpdateOutputs();
        }

        private void Menu_DoClick(object sender, EventArgs e)
        {
            UpdateOutputs();
        }

        protected override void AfterSolveInstance()
        {
            UpdateOutputs();
        }
        private void UpdateOutputs()
        {
            Type bhomObjectType = typeof(BHoMObject);
            Type bhomGeometryType = typeof(IBHoMGeometry);
            Type enumerableType = typeof(IEnumerable);

            List<string> keys = m_OutputTypes.Select(x => x.Name).ToList();

            int nbNew = keys.Count();
            int nbOld = Params.Output.Count();

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
                Params.Output[i].NickName = keys[i];

            for (int i = nbOld - 1; i > nbNew - 1; i--)
                Params.UnregisterOutputParameter(Params.Output[i]);

            for (int i = nbOld; i < nbNew; i++)
            {
                Type type = m_OutputTypes[i].PropertyType;
                bool isList = type != typeof(string) && (enumerableType.IsAssignableFrom(type)) && !typeof(IDictionary).IsAssignableFrom(type);

                if (isList)
                    type = type.GenericTypeArguments.First();

                if (bhomGeometryType.IsAssignableFrom(type))
                    Params.RegisterOutputParam(new Param_Geometry { NickName = keys[i] });
                else if (bhomObjectType.IsAssignableFrom(type))
                    Params.RegisterOutputParam(new BHoMObjectParameter { NickName = keys[i] });
                else
                    Params.RegisterOutputParam(new Param_GenericObject { NickName = keys[i] });
            }
            this.OnAttributesChanged();
            if (nbNew != nbOld)
                ExpireSolution(true);
        }




        private List<PropertyInfo> m_OutputTypes = new List<PropertyInfo>();
    }
}
