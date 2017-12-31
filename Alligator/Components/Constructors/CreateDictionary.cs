using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Collections;

namespace BH.UI.Alligator.Base
{
    public class CreateDictionary : CreateCustomTemplate
    {
        public CreateDictionary() : base("Create Dictionary", "Dictionary", "Create a dictionary from a list of keys and values", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.Dictionary; } }

        public override Guid ComponentGuid { get { return new Guid("6758EEE1-6A49-4D2B-A7FD-974383D3622E"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        public override bool CanInsertParameter(GH_ParameterSide side, int index) { return false; }
        public override bool CanRemoveParameter(GH_ParameterSide side, int index) { return false; }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("Keys", "Keys", "list of keys for the dictionary", GH_ParamAccess.list);
            pManager.AddScriptVariableParameter("Values", "Values", "list of values for the dictionary", GH_ParamAccess.list);

            base.RegisterInputParams(pManager);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DictionaryParameter(), "", "", "Dictionary", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<object> keys = GetListFromParameter(DA, 0);
            List<object> values = GetListFromParameter(DA, 1);

            if (keys.Count > 0 && values.Count == keys.Count)
            {
                Type keyType = keys.First().GetType();
                Type valueType = values.First().GetType();

                Type dicType = typeof(Dictionary<,>).MakeGenericType(new Type[] { keyType, valueType });
                IDictionary dic = (IDictionary) Activator.CreateInstance(dicType);
                for (int i = 0; i < keys.Count; i++)
                    dic.Add(keys[i], values[i]);

                DA.SetData(0, dic);
            }
        }
    }
}
