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

using BH.Engine.Rhinoceros;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Node2Code;
using System.Reflection;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
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
        /**** Private Methods                   ****/
        /*******************************************/

        private static INode ToNode(this GH_Component component, object selectedItem, string callerTypeName)
        {
            return null;
        }

        /*******************************************/

        private static T PopulateNode<T>(this T node, GH_Component component) where T: INode
        {
            node.Name = component.NickName;
            node.Description = component.Description;
            node.Inputs = component.Params.Input.Select(input => input.ToReceiverParam(component.InstanceGuid)).ToList();
            node.Outputs = component.Params.Output.Select(output => output.ToDataParam(component.InstanceGuid)).ToList();
            node.BHoM_Guid = component.InstanceGuid;

            return node;
        }

        /*******************************************/
    }
}
