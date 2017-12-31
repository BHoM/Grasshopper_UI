using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Collections;
using System.Windows.Forms;
using BH.oM.Base;
using BH.Engine.Reflection;
using BH.oM.Geometry;
using Grasshopper.Kernel.Parameters;
using BH.Engine.Rhinoceros;
using Grasshopper.Kernel.Types;

namespace BH.UI.Alligator.Base
{
    public class ExplodeJson : GH_Component, IGH_VariableParameterComponent
    {
        public ExplodeJson() : base("Explode", "Explode", "Explode an object or dictionary into child objects", "Alligator", "Base") { }
        public override Guid ComponentGuid { get { return new Guid("f2080175-a812-4dfb-86de-ae7dc8245668"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.Explode; } }
        public bool additional { get; set; }

        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }

        public bool CanInsertParameter(GH_ParameterSide side, int index) { return false; }
        public bool CanRemoveParameter(GH_ParameterSide side, int index) { return false; }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index) { return new Grasshopper.Kernel.Parameters.Param_GenericObject(); }
        public bool DestroyParameter(GH_ParameterSide side, int index) { return true; }
        public void VariableParameterMaintenance() { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "Object", "Object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object obj = null;
            if (!DA.GetData(0, ref obj) || obj == null)
                return;

            while (obj is IGH_Goo)
                obj = ((IGH_Goo)obj).ScriptVariable();

            Dictionary<string, object> outputs = new Dictionary<string, object>();
            if (obj is IDictionary)
                outputs = StringifyKeys(obj as dynamic); 
            else
                outputs = ((BHoMObject)obj).PropertyDictionary();

            if (obj is IDictionary)
                m_OutputTypes = outputs.Select(x => new Tuple<string, Type>(x.Key, x.Value.GetType())).ToList();
            else if (obj is CustomObject)
                m_OutputTypes = outputs.Select(x => new Tuple<string, Type>(x.Key, (x.Value == null) ? typeof(object) : x.Value.GetType())).ToList();
            else
                m_OutputTypes = obj.GetType().GetProperties().Where(x => x.CanRead && x.CanWrite).Select(x => new Tuple<string, Type>(x.Name, x.PropertyType)).ToList();

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
            if (Params.Output.Count() == 0)
                UpdateOutputs();
        }

        private void UpdateOutputs()
        {
            Type bhomObjectType = typeof(BHoMObject);
            Type bhomGeometryType = typeof(IBHoMGeometry);
            Type enumerableType = typeof(IEnumerable);

            List<string> keys = m_OutputTypes.Select(x => x.Item1).ToList();

            int nbNew = keys.Count();
            int nbOld = Params.Output.Count();

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
                Params.Output[i].NickName = keys[i];

            for (int i = nbOld - 1; i > nbNew - 1; i--)
                Params.UnregisterOutputParameter(Params.Output[i]);

            for (int i = nbOld; i < nbNew; i++)
            {
                Type type = m_OutputTypes[i].Item2;
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

        private Dictionary<string, object> StringifyKeys<TKey, TVal>(Dictionary<TKey, TVal> dic)
        {
            return dic.ToDictionary(x => x.Key.ToString(), x => x.Value as object);
        }

        private List<Tuple<string, Type>> m_OutputTypes = new List<Tuple<string, Type>>();
    }
}
