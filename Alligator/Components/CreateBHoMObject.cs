using System;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;
using System.Collections.Generic;
using BH.UI.Alligator;
using System.Linq;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Parameters.Hints;
using System.Runtime.CompilerServices;
using Grasshopper.Kernel.Data;
using Grasshopper;
using System.Collections;
using System.Windows.Forms;
using BH.oM.Base;
using System.Reflection;
using BH.oM.Geometry;

namespace BH.UI.Alligator.Base
{
    public class CreateBHoMObject : GH_Component, IGH_VariableParameterComponent
    {
        public CreateBHoMObject() : base("CreateBHoMObject", "BHoMObj", "Creates a specific class of BHoMObject", "Alligator", "Base") { }
        public override Guid ComponentGuid { get { return new Guid("0E1C95EB-1546-47D4-89BB-776F7920622D"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public BH.oM.Base.CustomObject customObj { get; set; } = new oM.Base.CustomObject();

        public bool CanInsertParameter(GH_ParameterSide side, int index) { return false; }
        public bool CanRemoveParameter(GH_ParameterSide side, int index) { return false; }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index) { return new Param_GenericObject(); }
        public bool DestroyParameter(GH_ParameterSide side, int index) { return true; }
        public void VariableParameterMaintenance() { }

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoMObject", "object", "BHoMObject", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (m_SelectedType == null)
            { 
                DA.SetData(0, null);
                return;
            }

            List<object> inputs = new List<object>();
            List<ParameterInfo> paramList = m_Constructor.GetParameters().ToList();
            for (int i = 0; i < m_Constructor.GetParameters().Length; i++)
                inputs.Add(m_DaGets[i].Invoke(null, new object[] { DA, i }));

            BHoMObject result = m_Constructor.Invoke(inputs.ToArray()) as BHoMObject;
            DA.SetData(0, result);
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            if (NickName == "BHoMObj")
            {
                System.Windows.Forms.ToolStripMenuItem typeMenu = Menu_AppendItem(menu, "Types");

                Type bhomType = typeof(BHoMObject);
                IEnumerable<Type> types = BH.Engine.Reflection.Create.TypeList().Where(x => x.IsSubclassOf(bhomType) && !x.ContainsGenericParameters).OrderBy(x => x.Name);
                m_TypeLinks = new Dictionary<ToolStripMenuItem, Type>();
                foreach (Type type in types)
                {
                    ToolStripMenuItem item = Menu_AppendItem(typeMenu.DropDown, type.Name, Item_Click);
                    m_TypeLinks[item] = type;
                }
            } 
        }

        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!m_TypeLinks.ContainsKey(item))
                return;

            Type type = m_TypeLinks[item];
            this.NickName = item.Text;
            m_SelectedType = m_TypeLinks[item];

            ConstructorInfo[] constructors = type.GetConstructors();
            m_Constructor = constructors[0];
            foreach (ConstructorInfo info in constructors)
            {
                ParameterInfo[] param = info.GetParameters();
                if (info.GetParameters().Length > m_Constructor.GetParameters().Length)
                    m_Constructor = info;
            }

            List<ParameterInfo> inputs = m_Constructor.GetParameters().ToList();
            ComputerDaGets(inputs);
            UpdateInputs(inputs);
        }


        private void UpdateInputs(List<ParameterInfo> inputs)
        {
            int nbNew = inputs.Count();
            int nbOld = Params.Input.Count();

            for (int i = nbOld - 1; i >= 0; i--)
                Params.UnregisterInputParameter(Params.Input[i]);

            for (int i = 0; i < nbNew; i++)
            {
                Type type = inputs[i].ParameterType;
                bool isList = (type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type)));

                if (isList)
                    type = type.GenericTypeArguments.First();

                RegisterInputParameter(type, inputs[i].Name);

                if (isList)
                    Params.Input[i].Access = GH_ParamAccess.list;
            }
            this.OnAttributesChanged();
            if (nbNew != nbOld)
                ExpireSolution(true);
        }

        private void ComputerDaGets(List<ParameterInfo> inputs)
        {
            int nbNew = inputs.Count();

            MethodInfo[] methods = typeof(IGH_DataAccess).GetMethods();
            MethodInfo getMethod = GetType().GetMethod("GetData");
            MethodInfo getListMethod = GetType().GetMethod("GetDataList");

            m_DaGets = new List<MethodInfo>();
            for (int i = 0; i < nbNew; i++)
            {
                Type type = inputs[i].ParameterType;
                bool isList = (type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type)));

                if (isList)
                    type = type.GenericTypeArguments.First();

                MethodInfo method = isList ? getListMethod : getMethod;
                m_DaGets.Add(method.MakeGenericMethod(type));
            }
        }


        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_SelectedType != null && m_Constructor != null)
            {
                writer.SetString("TypeName", m_SelectedType.FullName);
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            string typeString = "";
            reader.TryGetString("TypeName", ref typeString);

            m_SelectedType = BH.Engine.Reflection.Create.Type(typeString);

            ConstructorInfo[] constructors = m_SelectedType.GetConstructors();
            m_Constructor = constructors[0];
            foreach (ConstructorInfo info in constructors)
            {
                ParameterInfo[] param = info.GetParameters();
                if (info.GetParameters().Length > m_Constructor.GetParameters().Length)
                    m_Constructor = info;
            }

            ComputerDaGets(m_Constructor.GetParameters().ToList());

            return base.Read(reader);
        }

        private void RegisterInputParameter(Type type, string name)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(type))
                Params.RegisterInputParam(new BHoMGeometryParameter { NickName = name });
            else if (typeof(IObject).IsAssignableFrom(type))
                Params.RegisterInputParam(new BHoMObjectParameter { NickName = name });
            else if (type == typeof(string))
                Params.RegisterInputParam(new Param_String { NickName = name });
            else if (type == typeof(int))
                Params.RegisterInputParam(new Param_Integer { NickName = name });
            else if (type == typeof(double))
                Params.RegisterInputParam(new Param_Number { NickName = name });
            else if (type == typeof(bool))
                Params.RegisterInputParam(new Param_Boolean { NickName = name });
            else
                Params.RegisterInputParam(new Param_GenericObject { NickName = name });
        }


        public static T GetData<T>(IGH_DataAccess DA, int index)
        {
            T obj = default(T);
            DA.GetData<T>(index, ref obj);
            return obj;
        }


        public static List<T> GetDataList<T>(IGH_DataAccess DA, int index)
        {
            List<T> obj = new List<T>();
            DA.GetDataList<T>(index, obj);
            return obj;
        }

        private List<MethodInfo> m_DaGets = new List<MethodInfo>();

        private Type m_SelectedType = null;
        private ConstructorInfo m_Constructor = null;
        private Dictionary<ToolStripMenuItem, Type> m_TypeLinks = new Dictionary<ToolStripMenuItem, Type>();

        
    }
}