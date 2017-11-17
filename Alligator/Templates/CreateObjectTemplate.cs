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
//
//

namespace BH.UI.Alligator.Templates
{
    public abstract class CreateObjectTemplate : MethodCallTemplate
    {
        /*************************************/
        /**** Parameters Handling         ****/
        /*************************************/

        protected CreateObjectTemplate(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) { }

        /*public bool CanInsertParameter(GH_ParameterSide side, int index) { return false; }
        public bool CanRemoveParameter(GH_ParameterSide side, int index) { return false; }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index) { return new Param_GenericObject(); }
        public bool DestroyParameter(GH_ParameterSide side, int index) { return true; }
        public void VariableParameterMaintenance() { }

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }*/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)    // 1. Override this method if you want a different type of output
        {
            pManager.AddGenericParameter("Object", "object", "Object", GH_ParamAccess.item);    
        }


        /*************************************/
        /**** Solving Instance            ****/
        /*************************************/

        /*protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (m_Constructor == null)
            { 
                DA.SetData(0, null);
                return;
            }

            List<object> inputs = new List<object>();
            List<ParameterInfo> paramList = m_Constructor.GetParameters().ToList();
            for (int i = 0; i < m_Constructor.GetParameters().Length; i++)
                inputs.Add(m_DaGets[i].Invoke(null, new object[] { DA, i }));

            if (m_Constructor.IsConstructor)
                SetData(DA, ((ConstructorInfo)m_Constructor).Invoke(inputs.ToArray()));
            else
                SetData(DA, m_Constructor.Invoke(null, inputs.ToArray()));
        }*/

        /*************************************/

        /*protected virtual void SetData(IGH_DataAccess DA, object result)    // 2. Override this method if you want a different type of output
        {
            DA.SetData(0, result);
        }*/


        /*************************************/
        /**** Saving Component            ****/
        /*************************************/

        /*public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if ( m_Constructor != null)
            {
                ParameterInfo[] parameters = m_Constructor.GetParameters();
                writer.SetString("TypeName", m_Constructor.DeclaringType.AssemblyQualifiedName);
                writer.SetString("MethodName", m_Constructor.Name);
                writer.SetInt32("NbParams", parameters.Count());
                for (int i = 0; i < parameters.Count(); i++)
                    writer.SetString("ParamType", i, parameters[i].ParameterType.AssemblyQualifiedName);
            }
            return base.Write(writer);
        }*/

        /*************************************/

        /*public override bool Read(GH_IO.Serialization.GH_IReader reader)
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
            m_Constructor = null;

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
                            m_Constructor = method;
                            break;
                        }
                                
                    }
                }
            }

            ComputerDaGets(m_Constructor.GetParameters().ToList());

            return base.Read(reader);
        }*/


        /*************************************/
        /**** Creating Menu               ****/
        /*************************************/

        /*protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            if (m_Constructor == null)
            {
                ToolStripMenuItem typeMenu = Menu_AppendItem(menu, "Types");

                List<MethodInfo> methods = BH.Engine.Reflection.Query.GetBHoMMethodList().Where(x => x.DeclaringType.Name == "Create").ToList();

                foreach (var group in GetRelevantTypes().GroupBy(x => x.FullName.Split('.')[2]).OrderBy(x => x.Key))
                {
                    ToolStripMenuItem groupItem = Menu_AppendItem(typeMenu.DropDown, group.Key);

                    foreach (Type type in group)
                    {
                        MethodBase[] constructors = type.GetConstructors();
                        try
                        {
                            List<MethodInfo> m2 = methods.Where(x => x.ReturnType == type).ToList();
                            constructors = constructors.Concat(m2).ToArray();
                        }
                        catch(Exception)
                        {
                            Console.WriteLine("Failed to load some object definitions. Make sure everything in the chain has been recompiled");
                        }
                        
                        if (constructors.Count() > 2)
                        {
                            ToolStripMenuItem typeItem = Menu_AppendItem(groupItem.DropDown, type.Name);
                            foreach (MethodBase method in constructors)
                            {
                                if (method.GetParameters().Count() > 0)
                                {
                                    string name = "(" + method.GetParameters().Select(x => x.Name).Aggregate((x, y) => x + ", " + y) + ")";
                                    if (!method.IsConstructor && method.Name != type.Name)
                                        name = method.Name + name;
                                    ToolStripMenuItem item = Menu_AppendItem(typeItem.DropDown, name, Item_Click);
                                    m_ConstructorLinks[item] = method;
                                }
                            }
                        }
                        else
                        {
                            ToolStripMenuItem typeItem = Menu_AppendItem(groupItem.DropDown, type.Name, Item_Click);
                            m_ConstructorLinks[typeItem] = constructors.OrderBy(x => x.GetParameters().Count()).Last();
                        }
                        
                    }
                }
            } 
        }*/

        /*************************************/

        protected override Tree<MethodBase> GetRelevantMethods()
        {
            Tree<MethodBase> root = new Tree<MethodBase> { Name = "Create methods" };
            List<MethodInfo> createObjectMethods = BH.Engine.Reflection.Query.GetBHoMMethodList().Where(x => x.DeclaringType.Name == "Create").ToList();

            foreach (Type type in GetRelevantTypes().OrderBy(x => x.Name))
            {
                // Make sure the part of the tree corresponding to the namespace exists
                Tree<MethodBase> tree = root;
                foreach (string part in type.Namespace.Split('.').Skip(2))
                {
                    if (!tree.Children.ContainsKey(part))
                        tree.Children.Add(part, new Tree<MethodBase> { Name = part });
                    tree = tree.Children[part];
                }

                // Create the list of methods available to create this object type
                MethodBase[] methods = type.GetConstructors();
                try
                {
                    List<MethodInfo> m2 = createObjectMethods.Where(x => x.ReturnType == type).ToList();
                    methods = methods.Concat(m2).ToArray();
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to load some object definitions. Make sure everything in the chain has been recompiled");
                }

                // Add the methods to the tree
                if (methods.Count() > 2)
                {
                    tree.Children.Add(type.Name, new Tree<MethodBase> { Name = type.Name });
                    tree = tree.Children[type.Name];

                    foreach (MethodBase method in methods)
                    {
                        string name = (!method.IsConstructor && method.Name != type.Name) ? method.Name : "";
                        name = GetMethodString(name, method.GetParameters());
                        tree.Children.Add(name, new Tree<MethodBase>(method, name));
                    }
                }
                else
                {
                    MethodBase method = methods.OrderBy(x => x.GetParameters().Count()).Last();
                    tree.Children.Add(type.Name, new Tree<MethodBase>(method, type.Name));
                }
            }

            return root;
        }

        /*************************************/

        protected abstract IEnumerable<Type> GetRelevantTypes();    // 3. Define the list of relevant types of objects that can be created

        /*************************************/

        /*protected void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!m_ConstructorLinks.ContainsKey(item))
                return;

            m_Constructor = m_ConstructorLinks[item];
            this.NickName = m_Constructor.DeclaringType.Name;

            List<ParameterInfo> inputs = m_Constructor.GetParameters().ToList();
            ComputerDaGets(inputs);
            UpdateInputs(inputs);
        }*/


        /*************************************/
        /**** Dynamic Update              ****/
        /*************************************/

        /*protected void UpdateInputs(List<ParameterInfo> inputs)
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
        }*/

        /*************************************/

        /*protected void ComputerDaGets(List<ParameterInfo> inputs)
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
        }*/

        /*************************************/

        /*protected void RegisterInputParameter(Type type, string name, object defaultVal = null)
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
        }*/


        /*************************************/
        /**** Access Methods              ****/
        /*************************************/

        /*public static T GetData<T>(IGH_DataAccess DA, int index)
        {
            T obj = default(T);
            DA.GetData<T>(index, ref obj);
            return obj;
        }*/

        /*************************************/

        /*public static List<T> GetDataList<T>(IGH_DataAccess DA, int index)
        {
            List<T> obj = new List<T>();
            DA.GetDataList<T>(index, obj);
            return obj;
        }*/


        /*************************************/
        /**** Protected Fields            ****/
        /*************************************/

        /*protected List<MethodInfo> m_DaGets = new List<MethodInfo>();
        protected MethodBase m_Constructor = null;
        protected Dictionary<ToolStripMenuItem, MethodBase> m_ConstructorLinks = new Dictionary<ToolStripMenuItem, MethodBase>();*/

    }
}