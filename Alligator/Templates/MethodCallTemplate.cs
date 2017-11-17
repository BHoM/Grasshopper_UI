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
using BH.oM.DataStructure;

// Instructions to implement this template
// ***************************************
//
// If your output type is not a generic type
//      1. Override the RegisterOutputParams method
//      2. Override the SetData method
//
// In all cases
//      3. Override the GetRelevantTypes() method that provide the lis of types this component can create
//         If you need help building your catalogue of methods you can use AddMethodToTree() 
// 
//

namespace BH.UI.Alligator.Templates
{
    public abstract class MethodCallTemplate : GH_Component, IGH_VariableParameterComponent
    {
        /*************************************/
        /**** Parameters Handling         ****/
        /*************************************/

        protected MethodCallTemplate(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) { }

        public bool CanInsertParameter(GH_ParameterSide side, int index) { return false; }
        public bool CanRemoveParameter(GH_ParameterSide side, int index) { return false; }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index) { return new Param_GenericObject(); }
        public bool DestroyParameter(GH_ParameterSide side, int index) { return true; }
        public void VariableParameterMaintenance() { }

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)    // 1. Override this method if you want a different type of output
        {
            pManager.AddGenericParameter("Result", "result", "result", GH_ParamAccess.item);    
        }


        /*************************************/
        /**** Solving Instance            ****/
        /*************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (m_Method == null)
            { 
                DA.SetData(0, null);
                return;
            }

            List<object> inputs = new List<object>();
            for (int i = 0; i < m_DaGets.Count; i++)
                inputs.Add(m_DaGets[i].Invoke(null, new object[] { DA, i }));

            if (m_Method.IsConstructor)
                SetData(DA, ((ConstructorInfo)m_Method).Invoke(inputs.ToArray()));
            else
                SetData(DA, m_Method.Invoke(null, inputs.ToArray()));
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
            if ( m_Method != null)
            {
                ParameterInfo[] parameters = m_Method.GetParameters();
                writer.SetString("TypeName", m_Method.DeclaringType.AssemblyQualifiedName);
                writer.SetString("MethodName", m_Method.Name);
                writer.SetInt32("NbParams", parameters.Count());
                for (int i = 0; i < parameters.Count(); i++)
                    writer.SetString("ParamType", i, parameters[i].ParameterType.AssemblyQualifiedName);
            }
            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            string typeString = ""; reader.TryGetString("TypeName", ref typeString);
            string methodName = ""; reader.TryGetString("MethodName", ref methodName);
            int nbParams = 0; reader.TryGetInt32("NbParams", ref nbParams);

            List<Type> paramTypes = new List<Type>();
            for(int i = 0; i < nbParams; i++)
            {
                string paramType = ""; reader.TryGetString("ParamType", i, ref paramType);
                paramTypes.Add(Type.GetType(paramType));
            }

            Type type = Type.GetType(typeString);
            m_Method = null;

            List<MethodBase> methods;
            if (methodName == ".ctor")
                methods = type.GetConstructors().ToList<MethodBase>();
            else
                methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList<MethodBase>();

            foreach (MethodBase method in methods)
            {
                if (method.Name == methodName)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == paramTypes.Count)
                    {
                        bool matching = true;
                        for (int i = 0; i < paramTypes.Count; i++)
                        {
                            matching &= (parameters[i].ParameterType == paramTypes[i]);
                        }
                        if (matching)
                        {
                            m_Method = method;
                            break;
                        }
                                
                    }
                }
            }

            ComputerDaGets(m_Method.GetParameters().ToList());

            return base.Read(reader);
        }


        /*************************************/
        /**** Creating Menu               ****/
        /*************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            if (m_Method == null)
            {
                AppendMethodTreeToMenu(GetRelevantMethods(), menu);
            } 
        }

        /*************************************/

        protected void AppendMethodTreeToMenu(Tree<MethodBase> tree, ToolStripDropDown menu)
        {
            if (tree.Children.Count > 0)
            {
                ToolStripMenuItem treeMenu = Menu_AppendItem(menu, tree.Name);
                foreach (Tree<MethodBase> childTree in tree.Children.Values)
                    AppendMethodTreeToMenu(childTree, treeMenu.DropDown);
            }
            else
            {
                MethodBase method = tree.Value;
                ToolStripMenuItem methodItem = Menu_AppendItem(menu, tree.Name, Item_Click);
                m_MethodLinks[methodItem] = tree.Value;
            }
        }

        /*************************************/

        protected abstract Tree<MethodBase> GetRelevantMethods();    // 3. Define the list of relevant methods that can be created

        /*************************************/

        protected virtual void AddMethodToTree(Tree<MethodBase> tree, IEnumerable<string> names, MethodBase method) // Helper function to build your catalogue of methods
        {
            Dictionary<string, Tree<MethodBase>> children = tree.Children;

            if (names.Count() == 0)
            {
                string name = method.Name;
                bool isIMethod = (name.Length > 1 && Char.IsUpper(name[1]));

                if (isIMethod)
                    name = name.Substring(1);

                name = GetMethodString(name, method.GetParameters());

                if (isIMethod || !children.ContainsKey(name))
                    children[name] = new Tree<MethodBase>(method, name);
            }
            else
            {
                string name = names.First();
                if (!children.ContainsKey(name))
                    children.Add(name, new Tree<MethodBase> { Name = name });
                AddMethodToTree(children[name], names.Skip(1), method);
            }
        }

        /*************************************/

        protected virtual string GetMethodString(string methodName, ParameterInfo[] parameters)   // Helper function to build a string representing your method
        {
            string name = methodName + "(";
            if (parameters.Length > 0)
                name += parameters.Select(x => x.Name).Aggregate((x, y) => x + ", " + y);
            name += ")";

            return name;
        }

        /*************************************/

        protected void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!m_MethodLinks.ContainsKey(item))
                return;

            m_Method = m_MethodLinks[item];
            this.NickName = m_Method.DeclaringType.Name;

            List<ParameterInfo> inputs = m_Method.GetParameters().ToList();
            ComputerDaGets(inputs);
            UpdateInputs(inputs);
        }


        /*************************************/
        /**** Dynamic Update              ****/
        /*************************************/

        protected void UpdateInputs(List<ParameterInfo> inputs)
        {
            int nbOld = Params.Input.Count();

            for (int i = nbOld - 1; i >= 0; i--)
                Params.UnregisterInputParameter(Params.Input[i]);

            for (int i = 0; i < inputs.Count(); i++)
            {
                ParameterInfo input = inputs[i];
                Type type = input.ParameterType;
                bool isList = (type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type)));

                if (isList)
                    type = type.GenericTypeArguments.First();

                if (input.HasDefaultValue)
                {
                    RegisterInputParameter(type, input.Name, input.DefaultValue);
                    Params.Input[i].Optional = true;
                }
                else
                    RegisterInputParameter(type, input.Name);

                if (isList)
                    Params.Input[i].Access = GH_ParamAccess.list;
            }
            this.OnAttributesChanged();
            if (inputs.Count() != nbOld)
                ExpireSolution(true);
        }

        /*************************************/

        protected void ComputerDaGets(List<ParameterInfo> inputs)
        {
            int nbNew = inputs.Count();

            MethodInfo getMethod = typeof(MethodCallTemplate).GetMethod("GetData");
            MethodInfo getListMethod = typeof(MethodCallTemplate).GetMethod("GetDataList");

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

        protected void RegisterInputParameter(Type type, string name, object defaultVal = null)
        {
            dynamic p;

            if (typeof(IBHoMGeometry).IsAssignableFrom(type))
                p = new BHoMGeometryParameter { NickName = name };
            else if (typeof(IObject).IsAssignableFrom(type))
                p = new BHoMObjectParameter { NickName = name };
            else if (type == typeof(string))
                p = new Param_String { NickName = name };
            else if (type == typeof(int))
                p = new Param_Integer { NickName = name };
            else if (type == typeof(double))
                p = new Param_Number { NickName = name };
            else if (type == typeof(bool))
                p = new Param_Boolean { NickName = name };
            else
                p = new Param_GenericObject { NickName = name };

            if (defaultVal != null)
                p.SetPersistentData(defaultVal);

            Params.RegisterInputParam(p);
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
        protected MethodBase m_Method = null;
        protected Dictionary<ToolStripMenuItem, MethodBase> m_MethodLinks = new Dictionary<ToolStripMenuItem, MethodBase>();

    }
}