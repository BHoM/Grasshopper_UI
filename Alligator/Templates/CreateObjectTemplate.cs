using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.oM.DataStructure;

// Instructions to implement this template
// ***************************************
//
//  1. Override the GetRelevantTypes() method that provide the list of types this component can create
//  2. Change the value of m_MenuMaxDepth to modify the maximum depth of the menu (do that in the constructor of your new class)


namespace BH.UI.Alligator.Templates
{
    public abstract class CreateObjectTemplate : MethodCallTemplate
    {
        /*************************************/
        /**** Parts to Implement          ****/
        /*************************************/

        protected abstract IEnumerable<Type> GetRelevantTypes();    // 1. Define the list of relevant types of objects that can be created

        /*************************************/

        protected int m_MenuMaxDepth = 10;  // 2. Change this value to modify the maximum depth of the menu


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        protected CreateObjectTemplate(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override Tree<MethodBase> GetRelevantMethods()
        {
            Tree<MethodBase> root = new Tree<MethodBase> { Name = "Select constructor" };
            List<MethodInfo> createObjectMethods = BH.Engine.Reflection.Query.BHoMMethodList().Where(x => x.DeclaringType.Name == "Create").ToList();

            foreach (Type type in GetRelevantTypes().OrderBy(x => x.Name))
            {
                // Make sure the part of the tree corresponding to the namespace exists
                Tree<MethodBase> tree = root;
                foreach (string part in type.Namespace.Split('.').Skip(2).Take(m_MenuMaxDepth))
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
                if (methods.Length > 2)
                {
                    if (!tree.Children.ContainsKey(type.Name))
                        tree.Children.Add(type.Name, new Tree<MethodBase> { Name = type.Name });
                    tree = tree.Children[type.Name];

                    foreach (MethodBase method in methods)
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length > 0)
                        {
                            string name = (!method.IsConstructor && method.Name != type.Name) ? method.Name : "";
                            name = GetMethodString(name, parameters);
                            if (!tree.Children.ContainsKey(name))
                                tree.Children.Add(name, new Tree<MethodBase> { Value = method, Name = name });
                        }
                    }
                }
                else if (methods.Length > 0)
                {
                    MethodBase method = methods.OrderBy(x => x.GetParameters().Count()).Last();
                    if (!tree.Children.ContainsKey(type.Name))
                        tree.Children.Add(type.Name, new Tree<MethodBase> { Value = method, Name = type.Name });
                }
            }

            return root;
        }

        /*************************************/
    }
}