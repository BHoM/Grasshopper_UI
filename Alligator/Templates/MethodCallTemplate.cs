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
using Grasshopper.GUI;
using BH.Engine.Rhinoceros;
using Grasshopper.Kernel.Types;

// Instructions to implement this template
// ***************************************
//
//  1. Override the GetRelevantMethods() method that provide the list of methods this component can call
//  2. If you need help building your catalogue of methods, you can use AddMethodToTree() 
//

namespace BH.UI.Alligator.Templates
{
    public abstract class MethodCallTemplate : GH_Component, IGH_VariableParameterComponent, IGH_InitCodeAware
    {
        /*************************************/
        /**** 1 . Methods to Implement    ****/
        /*************************************/

        protected abstract Tree<MethodBase> GetRelevantMethods();    // 1. Define the list of relevant methods that can be created


        /*************************************/
        /**** 2 . Helper Methods          ****/
        /*************************************/

        protected virtual void AddMethodToTree(Tree<MethodBase> tree, MethodBase method) //2. Helper function to build your catalogue of methods
        {
            ParameterInfo[] parameters = method.GetParameters();
            string typeName = "Global";
            if (parameters.Length > 0)
            {
                Type type = parameters[0].ParameterType;
                if (type.IsGenericType)
                    type = type.GenericTypeArguments.First();
                typeName = type.Name;
            }

            IEnumerable<string> path = method.DeclaringType.Namespace.Split('.').Skip(2).Concat(new string[] { typeName });
            AddMethodToTree(tree, path, method);
        }

        /*************************************/

        protected virtual void AddMethodToTree(Tree<MethodBase> tree, IEnumerable<string> path, MethodBase method)
        {
            Dictionary<string, Tree<MethodBase>> children = tree.Children;

            if (path.Count() == 0)
            {
                string name = method.Name;
                bool isIMethod = (name.Length > 1 && Char.IsUpper(name[1]));

                if (isIMethod)
                    name = name.Substring(1);

                name = GetMethodString(name, method.GetParameters());

                if (isIMethod || !children.ContainsKey(name))
                    children[name] = new Tree<MethodBase> { Value = method, Name = name };
            }
            else
            {
                string name = path.First();
                if (!children.ContainsKey(name))
                    children.Add(name, new Tree<MethodBase> { Name = name });
                AddMethodToTree(children[name], path.Skip(1), method);
            }
        }

        /*************************************/

        protected virtual string GetMethodString(string methodName, ParameterInfo[] parameters)   // 2. Helper function to build a string representing your method
        {
            string name = methodName + "(";
            if (parameters.Length > 0)
                name += parameters.Select(x => x.Name).Aggregate((x, y) => x + ", " + y);
            name += ")";

            return name;
        }


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
        protected override void RegisterOutputParams(GH_OutputParamManager pManager) { }


        /*************************************/
        /**** Solving Instance            ****/
        /*************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (m_Method == null)
                return;

            List<object> inputs = new List<object>();
            try
            {
                for (int i = 0; i < m_DaGets.Count; i++)
                    inputs.Add(m_DaGets[i].Invoke(null, new object[] { DA, i }));
            }
            catch(Exception e)
            {
                if (e.InnerException != null)
                    throw new Exception(e.InnerException.Message);
                else
                    throw new Exception(e.Message);
            }

            dynamic result;
            try
            {
                if (m_Method.IsConstructor)
                    result = ((ConstructorInfo)m_Method).Invoke(inputs.ToArray());
                else
                    result = m_Method.Invoke(null, inputs.ToArray());
            }
            catch (Exception e)
            {
                string message = "This component failed to run properly. Are you sure you have the correct type of inputs?\n Check their description for more details. Here is the error provided by the method:\n ";
                if (e.InnerException != null)
                    message += e.InnerException.Message;
                else
                    message += e.Message;
                throw new Exception(message);
            }

            if (Params.Output.Count > 0)
            {
                if (Params.Output[0].Access == GH_ParamAccess.item)
                {
                    if (result is IBHoMGeometry)
                        DA.SetData(0, ((IBHoMGeometry)result).IToRhino());
                    else
                        DA.SetData(0, result);
                }
                else
                {
                    if (Params.Output[0] is Param_Geometry)
                        DA.SetDataList(0, ((IEnumerable)result).Cast<IBHoMGeometry>().Select(x => x.IToRhino()));
                    else
                        DA.SetDataList(0, result as IEnumerable);
                }
            }
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

            RestoreMethod(Type.GetType(typeString), methodName, paramTypes);
            if (m_Method != null)
                ComputerDaGets(m_Method.GetParameters().ToList());

            return base.Read(reader);
        }

        /*************************************/

        public void RestoreMethod(Type type, string methodName, List<Type> paramTypes)
        {
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
        }


        /*************************************/
        /**** Creating Menu               ****/
        /*************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            if (m_Method == null)
            {
                Tree<MethodBase> methods = GetRelevantMethods();
                AppendMethodTreeToMenu(methods, menu);
                AppendSearchMenu(methods, menu);
            } 
        }

        /*************************************/

        protected void AppendSearchMenu(Tree<MethodBase> methods, ToolStripDropDown menu)
        {
            m_Menu = menu;
            m_MethodList = GetMethodList(methods);

            Menu_AppendSeparator(menu);
            ToolStripMenuItem label = Menu_AppendItem(menu, "Search");
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            m_SearchBox = Menu_AppendTextItem(menu, "", null, Search_TextChanged, false);
        }

        /*************************************/

        private void Search_TextChanged(GH_MenuTextBox sender, string text)
        {
            // Clear the old items
            foreach (ToolStripItem item in m_SearchResultItems)
                item.Dispose();
            m_SearchResultItems.Clear();

            // Add the new ones
            text = text.ToLower();
            string[] parts = text.Split(' ');
            m_SearchResultItems.Add(Menu_AppendSeparator(m_Menu));
            foreach (Tree<MethodBase> tree in m_MethodList.Where(x => parts.All(y => x.Name.ToLower().Contains(y))).Take(12).OrderBy(x => x.Name))
            {
                ToolStripMenuItem methodItem = Menu_AppendItem(m_Menu, tree.Name, Item_Click);
                m_SearchResultItems.Add(methodItem);
                m_MethodLinks[methodItem] = tree.Value;
            }
        }

        /*************************************/

        protected void AppendMethodTreeToMenu(Tree<MethodBase> tree, ToolStripDropDown menu)
        {
            if (tree.Children.Count > 0)
            {
                ToolStripMenuItem treeMenu = Menu_AppendItem(menu, tree.Name);
                foreach (Tree<MethodBase> childTree in tree.Children.Values.OrderBy(x => x.Name))
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

        protected IEnumerable<Tree<MethodBase>> GetMethodList(Tree<MethodBase> tree)
        {
            return tree.Children.Values.SelectMany(x => GetMethodList(x, ""));
        }

        /*************************************/

        protected IEnumerable<Tree<MethodBase>> GetMethodList(Tree<MethodBase> tree, string path)
        {
            if (path.Length > 0 && !tree.Name.StartsWith("("))
                path = path + '.';

            if (tree.Children.Count == 0)
                return new Tree<MethodBase>[] { new Tree<MethodBase> { Value = tree.Value, Name = path + tree.Name } };
            else
                return tree.Children.Values.SelectMany(x => GetMethodList(x, path+tree.Name));
        }

        /*************************************/

        protected void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!m_MethodLinks.ContainsKey(item))
                return;

            m_Method = m_MethodLinks[item];
            if (m_Method == null)
                return;

            this.NickName = m_Method.IsConstructor ? m_Method.DeclaringType.Name : m_Method.Name;

            List<ParameterInfo> inputs = m_Method.GetParameters().ToList();
            Type output = m_Method.IsConstructor ? m_Method.DeclaringType : ((MethodInfo)m_Method).ReturnType;
            ComputerDaGets(inputs);
            UpdateInputs(inputs, output);
        }


        /*************************************/
        /**** Dynamic Update              ****/
        /*************************************/

        protected void UpdateInputs(List<ParameterInfo> inputs, Type output)
        {
            Type enumerableType = typeof(IEnumerable);

            // Create the inputs
            for (int i = 0; i < inputs.Count(); i++)
            {
                ParameterInfo input = inputs[i];
                Type type = input.ParameterType;
                bool isDictionary = typeof(IDictionary).IsAssignableFrom(type);
                bool isList = type != typeof(string) && (enumerableType.IsAssignableFrom(type)) && !isDictionary;

                // Get the object type if in a list
                if (isList)
                {
                    if (type.GenericTypeArguments.Length > 0)
                        type = type.GenericTypeArguments.First();
                    else
                        type = typeof(object);
                }

                // Register the input parameter
                if (input.HasDefaultValue)
                {
                    RegisterInputParameter(type, input.Name, input.DefaultValue);
                    Params.Input[i].Optional = true;
                }
                else
                    RegisterInputParameter(type, input.Name);

                // Define the access type
                if (isList)
                    Params.Input[i].Access = GH_ParamAccess.list;

                // Update the input description
                if (isList)
                    Params.Input[i].Description = string.Format("{0} is a list of {1}", input.Name, type.FullName);
                else if (isDictionary)
                    Params.Input[i].Description = string.Format("{0} is a dictionary of {1} keys and {2} values", input.Name, type.FullName, input.ParameterType.GenericTypeArguments[1]);
                else
                    Params.Input[i].Description = string.Format("{0} is a {1}", input.Name, type.FullName);
            }

            // Create the output
            if (output != null)
            {
                if ((output != typeof(string) && (enumerableType.IsAssignableFrom(output))))
                {
                    RegisterOutputParameter(output.GenericTypeArguments.First());
                    Params.Output[0].Access = GH_ParamAccess.list;
                }
                else
                    RegisterOutputParameter(output);
            }

            // Refresh the component
            this.OnAttributesChanged();
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

                if (type.IsByRef)
                    type = type.GetElementType();

                bool isList = (type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type))) && !typeof(IDictionary).IsAssignableFrom(type);

                if (isList)
                {
                    if (type.GenericTypeArguments.Length > 0)
                        type = type.GenericTypeArguments.First();
                    else
                        type = typeof(object);
                }
                    

                MethodInfo method = isList ? getListMethod : getMethod;
                m_DaGets.Add(method.MakeGenericMethod(type));
            }
        }

        /*************************************/

        protected void RegisterInputParameter(Type type, string name, object defaultVal = null)
        {
            dynamic p = GetGH_Param(type, name);

            if (defaultVal != null)
                p.SetPersistentData(defaultVal);

            Params.RegisterInputParam(p);
        }

        /*************************************/

        protected void RegisterOutputParameter(Type type)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(type))
                Params.RegisterOutputParam(new Param_Geometry { NickName = "" });
            else
                Params.RegisterOutputParam(GetGH_Param(type, ""));
        }

        /*************************************/

        protected dynamic GetGH_Param(Type type, string name)
        {
            dynamic p;

            if (typeof(IBHoMGeometry).IsAssignableFrom(type))
                p = new BHoMGeometryParameter { NickName = name };
            else if (typeof(IBHoMObject).IsAssignableFrom(type))
                p = new BHoMObjectParameter { NickName = name };
            else if (type == typeof(Type))
                p = new TypeParameter { NickName = name };
            else if (type == typeof(string))
                p = new Param_String { NickName = name };
            else if (type == typeof(int))
                p = new Param_Integer { NickName = name };
            else if (type == typeof(double))
                p = new Param_Number { NickName = name };
            else if (type == typeof(bool))
                p = new Param_Boolean { NickName = name };
            else if (typeof(Enum).IsAssignableFrom(type))
                p = new EnumParameter { NickName = name };
            else if (typeof(IObject).IsAssignableFrom(type))
                p = new IObjectParameter { NickName = name };
            else
                p = new Param_GenericObject { NickName = name };

            return p;
        }


        /*************************************/
        /**** Access Methods              ****/
        /*************************************/

        public static T GetData<T>(IGH_DataAccess DA, int index)
        {
            IGH_Goo goo = null;
            DA.GetData(index, ref goo);
            return ConvertGoo<T>(goo);
        }

        /*************************************/

        public static List<T> GetDataList<T>(IGH_DataAccess DA, int index)
        {
            List<IGH_Goo> goo = new List<IGH_Goo>();
            DA.GetDataList<IGH_Goo>(index, goo);
            return goo.Select(x => ConvertGoo<T>(x)).ToList();
        }

        /*************************************/

        public static T ConvertGoo<T>(IGH_Goo goo)
        {
            if (goo == null)
                return default(T);

            // Get the data out of the Goo
            object data = goo.ScriptVariable();
            while (data is IGH_Goo)
                data = ((IGH_Goo)data).ScriptVariable();

            if (data == null)
                return default(T);

            // Convert the data to an acceptable format
            if (data is T)
                return (T)data;
            else
            {
                if (data.GetType().Namespace.StartsWith("Rhino.Geometry"))
                    data = Engine.Rhinoceros.Convert.ToBHoM(data as dynamic);
                return (T)(data as dynamic);
            }
        }


        /*************************************/
        /**** Initialisation via String   ****/
        /*************************************/

        public void SetInitCode(string code)
        {
            CustomObject methodInfo = Engine.Serialiser.Convert.FromJson(code) as CustomObject;
            Type type = Type.GetType(methodInfo.CustomData["TypeName"] as string);
            string methodName = methodInfo.CustomData["MethodName"] as string;
            List<Type> paramTypes = (methodInfo.CustomData["Parameters"] as List<object>).Select(x => Type.GetType(x as string)).ToList();

            RestoreMethod(type, methodName, paramTypes);
            if (m_Method == null)
                return;

            this.NickName = m_Method.IsConstructor ? m_Method.DeclaringType.Name : m_Method.Name;

            List<ParameterInfo> inputs = m_Method.GetParameters().ToList();
            Type output = m_Method.IsConstructor ? m_Method.DeclaringType : ((MethodInfo)m_Method).ReturnType;
            ComputerDaGets(inputs);
            UpdateInputs(inputs, output);
        }


        /*************************************/
        /**** Protected Fields            ****/
        /*************************************/

        protected List<MethodInfo> m_DaGets = new List<MethodInfo>();
        protected MethodBase m_Method = null;
        protected Dictionary<ToolStripMenuItem, MethodBase> m_MethodLinks = new Dictionary<ToolStripMenuItem, MethodBase>();
        protected List<ToolStripItem> m_SearchResultItems = new List<ToolStripItem>();
        ToolStripTextBox m_SearchBox;
        ToolStripDropDown m_Menu;
        IEnumerable<Tree<MethodBase>> m_MethodList = new List<Tree<MethodBase>>();

        /*************************************/
    }
}