/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using Grasshopper.Kernel;
using BH.UI.Grasshopper.Base;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Parameters;
using System.Collections;
using System.Windows.Forms;
using BH.oM.Base;
using System.Reflection;
using BH.oM.Geometry;
using BH.oM.Data.Collections;
using BH.Engine.Rhinoceros;
using Grasshopper.Kernel.Types;
using BH.Engine.Reflection;
using BH.Engine.Data;
using System.IO;
using BH.UI.Grasshopper.Base.NonComponents.Ports;
using Grasshopper;
using Grasshopper.Kernel.Data;
using BH.UI.Grasshopper.Base.NonComponents.Menus;
using BH.oM.Testing;
using BH.Engine.Grasshopper;


// Instructions to implement this template
// ***************************************
//
//  1. Define the MethodGroup property corresponding to the set of method to cover
//  2. If you need help building your catalogue of methods, you can use AddMethodToTree() 
//

namespace BH.UI.Grasshopper.Templates
{
    public abstract class MethodCallTemplate : GH_Component, IGH_VariableParameterComponent, IGH_InitCodeAware
    {
        /*************************************/
        /**** 1. Helper Properties        ****/
        /*************************************/

        public virtual string MethodGroup { get; set; } = "";

        public virtual bool GroupByName { get; set; } = true;

        public virtual bool ShortenBranches { get; set; } = false;

        public virtual object SelectedItem { get { return m_Method; } }


        /*************************************/
        /**** 2 . Helper Methods          ****/
        /*************************************/

        protected virtual IEnumerable<MethodBase> GetRelevantMethods()
        {
            IEnumerable<MethodBase> methods;
            if (MethodGroup != "")
                methods = BH.Engine.Reflection.Query.BHoMMethodList().Where(x => x.DeclaringType.Name == MethodGroup);
            else
                methods = BH.Engine.Reflection.Query.BHoMMethodList();

            return methods.Where(x => !x.IsNotImplemented() && !x.IsDeprecated());
        }


        /*************************************/
        /**** Constructors                ****/
        /*************************************/

        protected MethodCallTemplate(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory)
        {
            // Make sure the assemblies are loaded
            if (!m_AssemblyLoaded)
            {
                m_AssemblyLoaded = true;
                string folder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Grasshopper\Libraries\Grasshopper\";
                BH.Engine.Reflection.Compute.LoadAllAssemblies(folder);
            }

            //Create the method tree and method list
            if (m_MethodTreeStore.ContainsKey(nickname) && m_MethodListStore.ContainsKey(nickname))
            {
                m_MethodTree = m_MethodTreeStore[nickname];
                m_MethodList = m_MethodListStore[nickname];
            }
            else
            {
                List<string> ignore = new List<string> { "BH", "oM", "Engine" };
                if (MethodGroup != "")
                    ignore.Add(MethodGroup);

                List<MethodBase> methods = GetRelevantMethods().ToList();
                IEnumerable<string> paths = methods.Select(x => x.ToText(true));

                m_MethodTree = BH.Engine.Data.Create.Tree(methods, paths.Select(x => x.Split('.').Where(y => !ignore.Contains(y)).ToList()).ToList(), "Select " + MethodGroup + " methods");
                if (GroupByName)
                    m_MethodTree = GroupMethodsByName(m_MethodTree);
                if (ShortenBranches)
                    m_MethodTree = m_MethodTree.ShortenBranches();
                m_MethodList = paths.Zip(methods, (k, v) => new Tuple<string, MethodBase>(k, v)).ToList();

                m_MethodTreeStore[nickname] = m_MethodTree;
                m_MethodListStore[nickname] = m_MethodList;
            }
        }

        /*************************************/

        protected Tree<MethodBase> GroupMethodsByName(Tree<MethodBase> tree)
        {

            if (tree.Children.Count > 0)
            {
                if (tree.Children.Values.First().Value != null)
                {
                    var groups = tree.Children.Where(x => x.Key.IndexOf('(') > 0).GroupBy(x => x.Key.Substring(0, x.Key.IndexOf('(')));

                    Dictionary<string, Tree<MethodBase>> children = new Dictionary<string, Tree<MethodBase>>();
                    foreach (var group in groups)
                    {
                        if (group.Count() == 1)
                            children.Add(group.Key, new Tree<MethodBase> { Name = group.Key, Value = group.First().Value.Value });
                        else
                            children.Add(group.Key, new Tree<MethodBase> { Name = group.Key, Children = group.ToDictionary(x => x.Key, x => x.Value) });
                    }
                    tree.Children = children;
                }
                else
                {
                    foreach (var child in tree.Children.Values)
                        GroupMethodsByName(child);
                }
            }

            return tree;
        }


        /*************************************/
        /**** GH Properties               ****/
        /*************************************/

        public bool CanInsertParameter(GH_ParameterSide side, int index) { return false; }
        public bool CanRemoveParameter(GH_ParameterSide side, int index) { return false; }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index) { return new Param_GenericObject(); }
        public bool DestroyParameter(GH_ParameterSide side, int index) { return true; }
        public void VariableParameterMaintenance() { }

        protected override void RegisterInputParams(GH_InputParamManager pManager) { }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager) { }

        public override bool Obsolete { get { return m_IsDeprecated; } }


        /*************************************/
        /**** Solving Instance            ****/
        /*************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (m_LoadingError.Length > 0)
                throw new Exception("This component failed to load properly. Check that the corresponding method still exists. \nError: " + m_LoadingError);

            if (m_Method == null)
                return;

            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            foreach (string warn in m_LoadingWarnings)
            {
                Engine.Reflection.Compute.RecordWarning(warn);
            }

            List<object> inputs = new List<object>();
            try
            {
                for (int i = 0; i < m_DaGets.Count; i++)
                    inputs.Add(m_DaGets[i].Invoke(null, new object[] { DA, i }));
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    throw new Exception(e.InnerException.Message);
                else
                    throw new Exception(e.Message);
            }

            dynamic result = null;
            try
            {
                if (m_Method.IsConstructor)
                    result = ((ConstructorInfo)m_Method).Invoke(inputs.ToArray());
                else
                    result = m_Method.Invoke(null, inputs.ToArray());
            }
            catch (Exception e)
            {
                string message = "This component failed to run properly. Are you sure you have the correct type of inputs?\n" +
                                 "Check their description for more details. Here is the error provided by the method:\n";
                if (e.InnerException != null)
                    message += e.InnerException.Message;
                else
                    message += e.Message;
                BH.Engine.Reflection.Compute.RecordError(message);
            }

            try
            {
                m_DaSet.Invoke(null, new object[] { DA, result });
            }
            catch (Exception e)
            {
                string message = "This component failed to run properly. Output data is calculated but cannot be set.\n";
                if (e.InnerException != null)
                    message += e.InnerException.Message;
                else
                    message += e.Message;
                BH.Engine.Reflection.Compute.RecordError(message);
            }

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }


        /*************************************/
        /**** Saving Component            ****/
        /*************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            bool isMethodNull = m_Method == null;

            if ( m_StoredTypeName != null)
            {
                writer.SetString("TypeName", m_StoredTypeName);
            }
            else if (!isMethodNull)
            {
                writer.SetString("TypeName", m_Method.DeclaringType.AssemblyQualifiedName);
            }

            if (m_StoredMethodName != null)
            {
                writer.SetString("MethodName", m_StoredMethodName);
            }
            else if (!isMethodNull)
            {
                writer.SetString("MethodName", m_Method.Name);
            }

            if (m_StoredParamTypes.Count != 0)
            {
                writer.SetInt32("NbParams", m_StoredParamTypes.Count);
                for (int i = 0; i < m_StoredParamTypes.Count; i++)
                    writer.SetString("ParamType", i, m_StoredParamTypes[i]);
            }
            else if (!isMethodNull)
            {
                ParameterInfo[] parameters = m_Method.GetParameters();
                writer.SetInt32("NbParams", parameters.Length);
                for (int i = 0; i < parameters.Count(); i++)
                    writer.SetString("ParamType", i, parameters[i].ParameterType.AssemblyQualifiedName);
            }

            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            try
            {
                reader.TryGetString("TypeName", ref m_StoredTypeName);
                string typeString = m_StoredTypeName;

                reader.TryGetString("MethodName", ref m_StoredMethodName);
                string methodName = m_StoredMethodName;

                int nbParams = 0; reader.TryGetInt32("NbParams", ref nbParams);

                // Get the input types
                List<Type> paramTypes = new List<Type>();

                m_IsDeprecated = false;
                for (int i = 0; i < nbParams; i++)
                {
                    string paramType = ""; reader.TryGetString("ParamType", i, ref paramType);
                    m_StoredParamTypes.Add(paramType);

                    //Fix for namespace change in structure
                    if (paramType.Contains("oM.Structural"))
                    {
                        paramType = paramType.Replace("oM.Structural", "oM.Structure");
                        m_IsDeprecated = true;
                    }

                    Type parameterType = paramType.ToType();

                    if (parameterType == null)
                    {
                        m_LoadingWarnings.Add($"Component failed to deserialize correctly. The type {paramType} cannot be found and is set to null.\n" +
                                               "It is recommended to replace the component with a new one.");
                    }
                    paramTypes.Add(parameterType);
                }

                //Read from the base
                if (!base.Read(reader))
                    return false;

                // Restore the method
                Type methodType = typeString.ToType();
                RestoreMethod(methodType, methodName, paramTypes);


                if (m_Method == null)
                {
                    m_LoadingError = $"{methodType}.{methodName} not found.";
                }
                else // Restore the ports
                {
                    m_IsDeprecated |= m_Method.IsDeprecated();
                    m_IsNotImplemented = m_Method.IsNotImplemented();
                    m_IsReleased = m_Method.IsReleased();
                    Type outputType = (m_Method is MethodInfo) ? ((MethodInfo)m_Method).ReturnType : m_Method.DeclaringType;
                    ComputeDaGets(m_Method.GetParameters().ToList(), outputType);
                }
            }
            catch (Exception e)
            {
                m_LoadingError = e.ToString();
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "This component failed to load properly. Check that the corresponding method still exists. \nInternal Error: " + m_LoadingError);
            }

            return true;
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

            for (int k = 0; k < methods.Count; k++)
            {
                MethodBase method = methods[k];

                if (method.Name == methodName)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == paramTypes.Count)
                    {
                        if (method.ContainsGenericParameters && method is MethodInfo)
                        {
                            Type[] generics = method.GetGenericArguments().Select(x => PortDataType.GetTypeFromGenericParameters(x)).ToArray();
                            method = ((MethodInfo)method).MakeGenericMethod(generics);
                            parameters = method.GetParameters();
                        }

                        bool matching = true;
                        for (int i = 0; i < paramTypes.Count; i++)
                        {
                            matching &= (paramTypes[i] == null || parameters[i].ParameterType == paramTypes[i]);
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

        public MethodBase GetMethod()
        {
            return m_Method;
        }

        /*************************************/

        public TestData GetAllCurrentData(IGH_DataAccess DA)
        {
            if (m_Method == null)
                return null;

            List<object> inputs = new List<object>();
            object result = null;

            try
            {
                // Get the inputs
                for (int i = 0; i < m_DaGets.Count; i++)
                    inputs.Add(m_DaGets[i].Invoke(null, new object[] { DA, i }));

                // Get the output
                if (m_Method.IsConstructor)
                    result = ((ConstructorInfo)m_Method).Invoke(inputs.ToArray());
                else
                    result = m_Method.Invoke(null, inputs.ToArray());
            }
            catch { }

            return new TestData { Inputs = inputs, Output = result };
        }


        /*************************************/
        /**** Creating Menu               ****/
        /*************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            if (m_Method == null)
            {
                SelectorMenu<MethodBase> selector = new SelectorMenu<MethodBase>(menu, Item_Click);
                selector.AppendTree(m_MethodTree);
                selector.AppendSearchBox(m_MethodList);
            }
        }

        /*************************************/

        private void Item_Click(object sender, MethodBase method)
        {
            m_Method = method;
            if (m_Method == null)
                return;

            if (m_Method.ContainsGenericParameters && m_Method is MethodInfo)
            {
                Type[] generics = m_Method.GetGenericArguments().Select(x => PortDataType.GetTypeFromGenericParameters(x)).ToArray();
                m_Method = ((MethodInfo)m_Method).MakeGenericMethod(generics);
            }

            ApplyMethod(m_Method);
        }


        /*************************************/
        /**** Dynamic Update              ****/
        /*************************************/

        protected void ApplyMethod(MethodBase method)
        {
            if (method == null)
                return;

            this.NickName = method.IsConstructor ? method.DeclaringType.Name : method.Name;
            m_IsDeprecated = method.IsDeprecated();
            m_IsNotImplemented = method.IsNotImplemented();
            m_IsReleased = method.IsReleased();

            List<ParameterInfo> inputs = method.GetParameters().ToList();
            Type output = method.IsConstructor ? method.DeclaringType : ((MethodInfo)method).ReturnType;
            SetPorts(inputs, output);
        }

        /*************************************/

        protected void SetPorts(List<ParameterInfo> inputs, Type output)
        {
            UpdateInputs(inputs, output);
            ComputeDaGets(inputs, output);
            Refresh();
        }

        /*************************************/

        protected void UpdateInputs(List<ParameterInfo> inputs, Type output)
        {
            // First take care of the component's description. TODO: should be elsewhere in the long run
            if (m_Method != null)
            {
                string description = m_Method.Description();
                if (description != "")
                    Description = description;
            }

            // Create the inputs
            Type enumerableType = typeof(IEnumerable);

            for (int i = 0; i < inputs.Count(); i++)
            {
                ParameterInfo input = inputs[i];
                Type type = input.ParameterType;
                PortDataType portInfo = new PortDataType(type);

                // Register the input parameter
                if (input.HasDefaultValue)
                {
                    RegisterInputParameter(portInfo.DataType, input.Name, input.DefaultValue);
                    Params.Input[i].Optional = true;
                }
                else
                    RegisterInputParameter(portInfo.DataType, input.Name);

                // Define the access type
                Params.Input[i].Access = portInfo.AccessMode;

                // use the in-code description if any
                string description = input.Description();
                Params.Input[i].Description = (description.Length > 0) ? description + "\n" : "";

                // Update the input description
                if (portInfo.AccessMode == GH_ParamAccess.list)
                    Params.Input[i].Description += string.Format("{0} is a list of {1}", input.Name, type.ToText());
                else if (portInfo.AccessMode == GH_ParamAccess.tree)
                    Params.Input[i].Description += string.Format("{0} is a tree of {1}", input.Name, type.ToText());
                else if (typeof(IDictionary).IsAssignableFrom(portInfo.DataType))
                    Params.Input[i].Description += string.Format("{0} is a dictionary of {1} keys and {2} values", input.Name, type.ToText(), input.ParameterType.GenericTypeArguments[1].ToText());
                else
                    Params.Input[i].Description += string.Format("{0} is a {1}", input.Name, type.ToText());
            }

            // Create the output
            if (output != null)
            {
                Type nameType = output;
                while (nameType.GetGenericArguments().Count() > 0)
                    nameType = nameType.GetGenericArguments().First();
                PortDataType portInfo = new PortDataType(output);
                RegisterOutputParameter(portInfo.DataType);
                Params.Output[0].Access = portInfo.AccessMode;
                Params.Output[0].Description = m_Method.OutputDescription();
                Params.Output[0].NickName = nameType.Name.Substring(0, 1);
                Params.Output[0].Name = nameType.Name.Substring(0, 1);
            }
        }

        /*************************************/

        protected void Refresh()
        {
            // Refresh the component
            this.OnAttributesChanged();
            ExpireSolution(true);
        }

        /*************************************/

        protected void ComputeDaGets(List<ParameterInfo> inputs, Type outType)
        {
            // Compute the input accessors
            MethodInfo getMethod = typeof(MethodCallTemplate).GetMethod("GetData");
            MethodInfo getListMethod = typeof(MethodCallTemplate).GetMethod("GetDataList");
            MethodInfo getTreeMethod = typeof(MethodCallTemplate).GetMethod("GetDataTree");

            m_DaGets = new List<MethodInfo>();
            for (int i = 0; i < inputs.Count(); i++)
            {
                Type type = inputs[i].ParameterType;
                if (type.IsByRef)
                    type = type.GetElementType();

                PortDataType portInfo = new PortDataType(type);
                if (portInfo.AccessMode != GH_ParamAccess.tree)
                {
                    MethodInfo method = portInfo.AccessMode == GH_ParamAccess.item ? getMethod : getListMethod;
                    m_DaGets.Add(method.MakeGenericMethod(portInfo.DataType));
                }
                else
                {
                    MethodInfo method = getTreeMethod;
                    m_DaGets.Add(method.MakeGenericMethod(portInfo.DataType, Params.Input[i].Type));
                }
            }

            // Compute the output accessor
            if (outType.IsByRef)
                outType = outType.GetElementType();

            PortDataType outInfo = new PortDataType(outType);
            MethodInfo outMethod = outInfo.AccessMode == GH_ParamAccess.item ? typeof(MethodCallTemplate).GetMethod("SetData") :
                                outInfo.AccessMode == GH_ParamAccess.list ? typeof(MethodCallTemplate).GetMethod("SetDataList") :
                                typeof(MethodCallTemplate).GetMethod("SetDataTree");
            m_DaSet = outMethod.MakeGenericMethod(outInfo.DataType);
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
            if (typeof(IGeometry).IsAssignableFrom(type))
            {
                Params.RegisterOutputParam(new IGeometryParameter { NickName = "" });
            }
            else
                Params.RegisterOutputParam(GetGH_Param(type, ""));
        }

        /*************************************/

        protected dynamic GetGH_Param(Type type, string name)
        {
            dynamic p;

            if (typeof(IGeometry).IsAssignableFrom(type))
                p = new IGeometryParameter { NickName = name };
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

        public static List<List<T>> GetDataTree<T, PT>(IGH_DataAccess DA, int index) where PT : IGH_Goo
        {
            GH_Structure<PT> goo = new GH_Structure<PT>();
            DA.GetDataTree(index, out goo);
            return goo.Branches.Select(x => x.Select(y => ConvertGoo<T>(y)).ToList()).ToList();
        }

        /*************************************/

        public static bool SetData<T>(IGH_DataAccess DA, T data)
        {
            if (typeof(IGeometry).IsAssignableFrom(typeof(T)))
            {
                object result = ToRhino(data);
                if (result is IEnumerable)
                    return DA.SetDataList(0, result as IEnumerable);
                else
                    return DA.SetData(0, result);
            }
            else
                return DA.SetData(0, data);
        }

        /*************************************/

        public static bool SetDataList<T>(IGH_DataAccess DA, IEnumerable<T> data)
        {
            if (data != null && typeof(IGeometry).IsAssignableFrom(typeof(T)))
                return DA.SetDataList(0, data.Select(x => ToRhino(x)));
            else
                return DA.SetDataList(0, data);
        }

        /*************************************/

        public static bool SetDataTree<T>(IGH_DataAccess DA, IEnumerable<IEnumerable<T>> data)
        {
            if (typeof(IGeometry).IsAssignableFrom(typeof(T)))
                return DA.SetDataTree(0, BuildDataTree(data.Select(v => v.Select(x => (ToRhino(x)))).ToList(), DA.Iteration));
            else
                return DA.SetDataTree(0, BuildDataTree(data.ToList(), DA.Iteration));
        }

        /*************************************/

        private static object ToRhino(object x)
        {
            try
            {
                if (x == null)
                    return null;
                else if (BH.Engine.Rhinoceros.Query.IsRhinoEquivalent(x.GetType()))
                    return ((IGeometry)x).IToRhino();
                else
                    return x;
            }
            catch (Exception e)
            {
                BH.Engine.Reflection.Compute.RecordError("Object of type " + x.GetType().ToString() + " failed to convert to a Rhino geometry\nError: " + e.Message);
                return null;
            }
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
                    data = BH.Engine.Rhinoceros.Convert.ToBHoM(data as dynamic);
                return (T)(data as dynamic);
            }
        }

        /*************************************/

        public static DataTree<T> BuildDataTree<T>(List<IEnumerable<T>> data, int iteration)
        {
            DataTree<T> tree = new DataTree<T>();

            if (data.Count == 0)
                tree.EnsurePath(new GH_Path(iteration));

            for (int i = 0; i < data.Count; i++)
            {
                tree.AddRange(data[i], new GH_Path(iteration, i));
            }

            return tree;
        }


        /*************************************/
        /**** Initialisation via String   ****/
        /*************************************/

        public void SetInitCode(string code)
        {
            CustomObject methodInfo = BH.Engine.Serialiser.Convert.FromJson(code) as CustomObject;
            Type type = Engine.Grasshopper.Convert.ToType(methodInfo.CustomData["TypeName"] as string);
            string methodName = methodInfo.CustomData["MethodName"] as string;
            List<Type> paramTypes = (methodInfo.CustomData["Parameters"] as List<object>).Select(x => ((string)x == null) ? null : Engine.Grasshopper.Convert.ToType(x as string)).ToList();

            RestoreMethod(type, methodName, paramTypes);
            ApplyMethod(m_Method);
        }


        /*************************************/
        /**** Protected Fields            ****/
        /*************************************/

        // Method containers calculated once at construction (both for menu tree and search box)
        protected Tree<MethodBase> m_MethodTree = new Tree<MethodBase>();
        protected List<Tuple<string, MethodBase>> m_MethodList = new List<Tuple<string, MethodBase>>();

        // Method chosen by the user and corresponding input handlers
        protected MethodBase m_Method = null;
        protected List<MethodInfo> m_DaGets = new List<MethodInfo>();
        protected MethodInfo m_DaSet = null;
        protected string m_LoadingError = "";
        protected List<string> m_LoadingWarnings = new List<string>();

        protected bool m_IsDeprecated = false;
        protected bool m_IsNotImplemented = false;
        protected bool m_IsReleased = false;

        // Method as deserialized
        protected string m_StoredMethodName = "";
        protected string m_StoredTypeName = "";
        protected List<string> m_StoredParamTypes = new List<string>();


        /*************************************/
        /**** Static Fields               ****/
        /*************************************/

        private static bool m_AssemblyLoaded = false;
        private static Dictionary<string, Tree<MethodBase>> m_MethodTreeStore = new Dictionary<string, Tree<MethodBase>>();
        private static Dictionary<string, List<Tuple<string, MethodBase>>> m_MethodListStore = new Dictionary<string, List<Tuple<string, MethodBase>>>();


        /*************************************/
    }
}