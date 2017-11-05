using System;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Parameters;
using System.Collections;
using System.Windows.Forms;
using BH.oM.Base;
using System.Reflection;
using BH.oM.Geometry;

// Instructions to implement this template
// ***************************************
//
// If your output type is not a generic type
//      1. Override the RegisterOutputParams method
//      2. Override the SetData method
//
// In all cases
//      3. Override the GetRelevantTypes() method that provide the lis of types this component can create
//
//

namespace BH.UI.Alligator.Templates
{
    public abstract class CreateObjectTemplate : GH_Component, IGH_VariableParameterComponent
    {
        /*************************************/
        /**** Parameters Handling         ****/
        /*************************************/

        protected CreateObjectTemplate(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) { }

        public bool CanInsertParameter(GH_ParameterSide side, int index) { return false; }
        public bool CanRemoveParameter(GH_ParameterSide side, int index) { return false; }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index) { return new Param_GenericObject(); }
        public bool DestroyParameter(GH_ParameterSide side, int index) { return true; }
        public void VariableParameterMaintenance() { }

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)    // 1. Override this method if you want a different type of output
        {
            pManager.AddGenericParameter("Object", "object", "Object", GH_ParamAccess.item);    
        }


        /*************************************/
        /**** Solving Instance            ****/
        /*************************************/

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

            SetData(DA, m_Constructor.Invoke(inputs.ToArray()));
        }

        /*************************************/

        protected virtual void SetData(IGH_DataAccess DA, object result)    // 2. Override this method if you want a different type of output
        {
            DA.SetData(0, result);
        }


        /*************************************/
        /**** Saving Component            ****/
        /*************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_SelectedType != null && m_Constructor != null)
            {
                writer.SetString("TypeName", m_SelectedType.FullName);
            }
            return base.Write(writer);
        }

        /*************************************/

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


        /*************************************/
        /**** Creating Menu               ****/
        /*************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            if (m_SelectedType == null)
            {
                ToolStripMenuItem typeMenu = Menu_AppendItem(menu, "Types");

                m_TypeLinks = new Dictionary<ToolStripMenuItem, Type>();
                foreach (Type type in GetRelevantTypes())
                {
                    ToolStripMenuItem item = Menu_AppendItem(typeMenu.DropDown, type.Name, Item_Click);
                    m_TypeLinks[item] = type;
                }
            } 
        }

        /*************************************/

        protected abstract IEnumerable<Type> GetRelevantTypes();    // 3. Define the list of relevant types of objects that can be created

        /*************************************/

        protected void Item_Click(object sender, EventArgs e)
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


        /*************************************/
        /**** Dynamic Update              ****/
        /*************************************/

        protected void UpdateInputs(List<ParameterInfo> inputs)
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

                if (inputs[i].DefaultValue != null)
                    Params.Input[i].Optional = true;                    
            }
            this.OnAttributesChanged();
            if (nbNew != nbOld)
                ExpireSolution(true);
        }

        /*************************************/

        protected void ComputerDaGets(List<ParameterInfo> inputs)
        {
            int nbNew = inputs.Count();

            MethodInfo getMethod = typeof(CreateObjectTemplate).GetMethod("GetData");
            MethodInfo getListMethod = typeof(CreateObjectTemplate).GetMethod("GetDataList");

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

        /*************************************/

        protected void RegisterInputParameter(Type type, string name)
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


        /*************************************/
        /**** Access Methods              ****/
        /*************************************/

        public static T GetData<T>(IGH_DataAccess DA, int index)
        {
            T obj = default(T);
            DA.GetData<T>(index, ref obj);
            return obj;
        }

        /*************************************/

        public static List<T> GetDataList<T>(IGH_DataAccess DA, int index)
        {
            List<T> obj = new List<T>();
            DA.GetDataList<T>(index, obj);
            return obj;
        }


        /*************************************/
        /**** Protected Fields            ****/
        /*************************************/

        protected List<MethodInfo> m_DaGets = new List<MethodInfo>();

        protected Type m_SelectedType = null;
        protected ConstructorInfo m_Constructor = null;
        protected Dictionary<ToolStripMenuItem, Type> m_TypeLinks = new Dictionary<ToolStripMenuItem, Type>();

        
    }
}