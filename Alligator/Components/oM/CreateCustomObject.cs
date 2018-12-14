using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Parameters.Hints;
using System.Runtime.CompilerServices;
using BH.oM.Base;
using RG = Rhino.Geometry;
using System.Collections;
using BH.oM.DataStructure;
using BH.Engine.Reflection;
using System.Windows.Forms;
using BH.UI.Alligator.Base.NonComponents.Menus;
using System.Reflection;
using BH.UI.Alligator.Base.NonComponents.Ports;
using BH.Engine.Reflection.Convert;
using BH.Engine.Grasshopper;

namespace BH.UI.Alligator.Base
{
    public class CreateCustomObject : CreateCustomTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("dbd3fe50-423a-4ea4-8bc7-7ad94d1d67e9"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.CustomObject; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;

        public override bool Obsolete { get; } = true;

        public object SelectedType { get { return m_ForcedType; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateCustomObject() : base("Create Custom Object", "CustomObj", "Creates a custom BHoMObject from custom inputs", "Alligator", " oM")
        {
            // Make sure the assemblies are loaded
            if (!m_AssemblyLoaded)
            {
                m_AssemblyLoaded = true;
                string folder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Grasshopper\Libraries\Alligator\";
                BH.Engine.Reflection.Compute.LoadAllAssemblies(folder);
            }

            //Create the method tree and method list
            if (m_TypeTree == null && m_TypeList == null)
            {
                List<string> ignore = new List<string> { "BH", "oM", "Engine" };

                IEnumerable<Type> types = BH.Engine.Reflection.Query.BHoMTypeList();
                IEnumerable<string> paths = types.Select(x => x.ToText(true));

                m_TypeTree = BH.Engine.DataStructure.Create.Tree(types, paths.Select(x => x.Split('.').Where(y => !ignore.Contains(y))), "Select enforced Type");
                m_TypeList = paths.Zip(types, (k, v) => new Tuple<string, Type>(k, v)).ToList();
            }

        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }

        /*******************************************/

        public override bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }

        /*******************************************/

        public override void VariableParameterMaintenance()
        {
            base.VariableParameterMaintenance();

            for (int i = 0; i < Params.Input.Count; i++)
                Params.Input[i].Description = string.Format("Object Property {0}", Params.Input[i].NickName);
        }

        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("Name", "Name", "Name of the object", GH_ParamAccess.item);
            pManager.AddScriptVariableParameter("Tags", "Tags", "Tags of the object", GH_ParamAccess.list);
            ((Param_ScriptVariable)Params.Input[0]).TypeHint = new GH_StringHint_CS();
            ((Param_ScriptVariable)Params.Input[1]).TypeHint = new GH_StringHint_CS();
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            ((Param_ScriptVariable)pManager[0]).PersistentData.Append(new GH_String(""));

            base.RegisterInputParams(pManager);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoMObject", "object", "BHoMObject", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            IObject obj = new CustomObject();
            if (m_ForcedType != null)
                obj = Activator.CreateInstance(m_ForcedType) as IObject;
            if (obj == null)
                obj = new CustomObject();

            for (int i = 0; i < Params.Input.Count; i++)
            {
                if (Params.Input[i].SourceCount > 0)
                    BH.Engine.Reflection.Modify.SetPropertyValue(obj, Params.Input[i].NickName, GetInputData(DA, i));
            }
                
            DA.SetData(0, obj);

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_ForcedType != null)
            {
                writer.SetString("ForcedType", m_ForcedType.AssemblyQualifiedName);
            }
            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            try
            {
                //Read from the base
                if (!base.Read(reader))
                    return false;

                // Get the forced type if any
                string typeString = "";
                if (reader.TryGetString("ForcedType", ref typeString))
                {
                    //Fix for namespace change in structure
                    if (typeString.Contains("oM.Structural"))
                    {
                        typeString = typeString.Replace("oM.Structural", "oM.Structure");
                        //m_IsDeprecated = true;
                    }

                    m_ForcedType = typeString.ToType();
                }

                return true;
            }
            catch (Exception e)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "This component failed to load properly. \nInternal Error: " + e.ToString());
                return false;
            }
        }


        /*************************************/
        /**** Creating Menu               ****/
        /*************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            if (m_ForcedType == null)
            {
                SelectorMenu<Type> selector = new SelectorMenu<Type>(menu, Item_Click);
                selector.AppendTree(m_TypeTree);
                selector.AppendSearchBox(m_TypeList);
            }
        }

        /*************************************/

        private void Item_Click(object sender, Type type)
        {
            m_ForcedType = type;
            if (m_ForcedType == null)
                return;

            ApplyType(type);
        }


        /*************************************/
        /**** Dynamic Update              ****/
        /*************************************/

        protected void ApplyType(Type type)
        {
            if (type == null)
                return;

            this.NickName = type.Name;

            for (int i = Params.Input.Count - 1; i >= 0; i--)
                Params.UnregisterInputParameter(Params.Input[i]);
                

            foreach (PropertyInfo prop in type.GetProperties().Where(x => x.Name != "CustomData"))
            {
                PortDataType portInfo = new PortDataType(prop.PropertyType);
                Params.RegisterInputParam(new Param_ScriptVariable { NickName = prop.Name, Optional = true, Access = portInfo.AccessMode });
            }
                

            this.OnAttributesChanged();
            ExpireSolution(true);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private object ToBHoM(object obj)
        {
            if (obj is IList)
            {
                return ((IList)obj).Cast<object>().Select(x => ToBHoM(x)).ToList();
            }
            else if (obj.GetType().Namespace.StartsWith("Rhino"))
            {
                try
                {
                    return BH.Engine.Rhinoceros.Convert.ToBHoM(obj as dynamic);
                }
                catch (Exception)
                {
                    return obj;
                }
            }
            else
                return obj;
        }

        /*************************************/

        private object GetInputData(IGH_DataAccess DA, int index)
        {
            switch (this.Params.Input[index].Access)
            {
                case GH_ParamAccess.item:
                    return ToBHoM(RuntimeHelpers.GetObjectValue(GetItemFromParameter(DA, index)));
                case GH_ParamAccess.list:
                    return ToBHoM(RuntimeHelpers.GetObjectValue(GetListFromParameter(DA, index)));
                case GH_ParamAccess.tree:
                    return ToBHoM(RuntimeHelpers.GetObjectValue(GetTreeFromParameter(DA, index)));
                default:
                    return null;
            }
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private static bool m_AssemblyLoaded = false;
        private static Tree<Type> m_TypeTree = null;
        private static List<Tuple<string, Type>> m_TypeList = null;
        private Type m_ForcedType = null;


        /*******************************************/
    }
}