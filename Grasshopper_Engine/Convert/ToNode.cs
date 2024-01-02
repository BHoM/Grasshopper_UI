/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using BH.Engine.Rhinoceros;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Programming;
using System.Reflection;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using BH.oM.Base;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public static INode IToNode(this GH_Component component, object selectedItem = null, string callerTypeName = "")
        {
            if (selectedItem != null)
                return ToNode(component as dynamic, selectedItem as dynamic, callerTypeName);
            else if (callerTypeName.Length > 0)
                return ToNode(component as dynamic, callerTypeName);
            else
                return ToNode(component as dynamic);
        }

        /*******************************************/

        public static INode IToNode<T>(this GH_Param<T> component, List<object> choices = null, object selectedItem = null) where T : class, IGH_Goo
        {
            if (choices == null)
                return ToNode(component as dynamic);
            else
                return ToNode(component as dynamic, choices, selectedItem);
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static INode ToNode(this GH_Component component, MethodInfo method, string callerTypeName)
        {
            switch (callerTypeName)
            {
                case "GetPropertyCaller":
                    return PopulateNode(new GetPropertyNode(), component);
                case "SetPropertyCaller":
                    return PopulateNode(new SetPropertyNode(), component);
                default:
                    return PopulateNode(new MethodNode { Method = method }, component);
            }
        }

        /*******************************************/

        public static ConstructorNode ToNode(this GH_Component component, ConstructorInfo constructor, string callerTypeName)
        {
            return PopulateNode(new ConstructorNode { Constructor = constructor }, component);
        }

        /*******************************************/

        public static INode ToNode(this GH_Component component, Type type, string callerTypeName)
        {
            switch(callerTypeName)
            {
                case "CreateObjectCaller":
                    return PopulateNode(new InitialiserNode { ObjectType = type }, component);  
                case "CreateTypeCaller":
                    return PopulateNode(new TypeNode { Type = type }, component);
                default:
                    return null;
            }
        }

        /*******************************************/

        public static INode ToNode(this GH_Component component, string callerTypeName)
        {
            switch (callerTypeName)
            {
                case "ExplodeCaller":
                    return PopulateNode(new ExplodeNode(), component);
                case "CreateCustomCaller":
                    return PopulateNode(new CustomObjectNode(), component);
                default:
                    return null;
            }
        }

        /*******************************************/

        public static INode ToNode(this GH_Component component)
        {
            switch (component.GetType().FullName)
            {
                case "MathComponents.FunctionComponents.Component_Series":
                    MethodNode node = PopulateNode(new MethodNode { Method = typeof(BH.Engine.Data.Compute).GetMethod("Series") }, component);
                    node.Inputs = new List<ReceiverParam> { node.Inputs[2], node.Inputs[0], node.Inputs[1] };
                    return node;
                default:
                    return null;
            }
        }

        /*******************************************/

        public static INode ToNode(this GH_ValueList component, List<object> choices, object selectedItem)
        {
            int index = 0;
            bool success = component.SelectedItems.First().Value.CastTo<int>(out index);
            if (success && index >= 0 && index < choices.Count)
            {
                object choice = choices[index];
                if (component.GetType().Name == "CreateDataComponent")
                {
                    DataParam parameter = PopulateParam(new DataParam { Data = choice as BHoMObject, DataType = choice.GetType() }, component);
                    return new LibraryNode
                    {
                        SourceFile = selectedItem as string,
                        Name = parameter.Name,
                        Description = parameter.Description,
                        BHoM_Guid = parameter.BHoM_Guid,
                        Inputs = new List<ReceiverParam>(),
                        Outputs = new List<DataParam> { parameter }
                    };
                }
                else
                    return ParamNode(PopulateParam(new DataParam { Data = choice, DataType = choice.GetType() }, component));
            }
            else
            {
                return null;
            }
        }

        /*******************************************/

        public static INode ToNode(this GH_Panel component)
        {
            double number;
            if (double.TryParse(component.UserText, out number))
            {
                Type type = (number % 1 == 0) ? typeof(int) : typeof(double);
                return ParamNode(PopulateParam(new DataParam { Data = number, DataType = type }, component));
            }
            else
                return ParamNode(PopulateParam(new DataParam { Data = component.UserText, DataType = typeof(string) }, component));
        }

        /*******************************************/

        public static INode ToNode(this GH_NumberSlider component)
        {
            double value = (double)component.CurrentValue;
            Type type = (value % 1 == 0) ? typeof(int) : typeof(double);

            return ParamNode(PopulateParam(new DataParam { Data = value, DataType = type }, component));
        }

        /*******************************************/

        public static INode ToNode(this GH_BooleanToggle component)
        {
            return ParamNode(PopulateParam(new DataParam { Data = component.Value, DataType = typeof(bool) }, component));
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static INode ToNode(this GH_Component component, object selectedItem, string callerTypeName)
        {
            return null;
        }

        /*******************************************/

        public static INode ToNode<T>(this GH_Param<T> component, List<object> choices = null, object selectedItem = null) where T : class, IGH_Goo
        {
            return null;
        }

        /*******************************************/

        public static INode ToNode<T>(this GH_Param<T> component) where T : class, IGH_Goo
        {
            return null;
        }

        /*******************************************/

        private static T PopulateNode<T>(this T node, GH_Component component) where T: INode
        {
            node.Name = component.NickName;
            node.Description = component.Description;
            node.Inputs = component.Params.Input.Select(input => input.ToReceiverParam()).ToList();
            node.Outputs = component.Params.Output.Select(output => output.ToDataParam()).ToList();
            node.BHoM_Guid = component.InstanceGuid;

            return node;
        }

        /*******************************************/

        private static DataParam PopulateParam(this DataParam parameter, IGH_Param component)
        {
            parameter.Name = component.NickName;
            parameter.Description = component.Description;
            parameter.BHoM_Guid = component.InstanceGuid;
            parameter.TargetIds = component.Recipients.Select(r => r.InstanceGuid).ToList();

            return parameter;
        }

        /*******************************************/

        private static ParamNode ParamNode(this DataParam parameter)
        {
            return new ParamNode
            {
                Name = parameter.Name,
                Description = parameter.Description,
                BHoM_Guid = parameter.BHoM_Guid,
                Inputs = new List<ReceiverParam>(),
                Outputs = new List<DataParam> { parameter }
            };
        }

        /*******************************************/
    }
}




