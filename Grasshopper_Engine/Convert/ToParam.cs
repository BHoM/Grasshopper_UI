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
using BH.oM.Base;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static DataParam IToParam<T>(this GH_Param<T> component, List<object> choices = null, object selectedItem = null) where T : class, IGH_Goo
        {
            if (choices == null)
                return ToParam(component as dynamic);
            else
                return ToParam(component as dynamic, choices, selectedItem);
        }

        /*******************************************/

        public static DataParam ToParam(this GH_ValueList component, List<object> choices, object selectedItem)
        {
            bool success = component.SelectedItems.First().Value.CastTo<int>(out int index);
            if (success && index >= 0 && index < choices.Count)
            {
                object choice = choices[index];
                if (component.GetType().Name == "CreateDataComponent")
                    return PopulateParam(new DataParam { Data = new LibraryData { Data = choice as BHoMObject, SourceFile = selectedItem as string }, DataType = choice.GetType() }, component);
                else
                    return PopulateParam(new DataParam { Data = choice, DataType = choice.GetType() }, component);
            }
            else
            {
                return null;
            }  
        }

        /*******************************************/

        public static DataParam ToParam(this GH_Panel component)
        {
            double number;
            if (double.TryParse(component.UserText, out number))
            {
                Type type = (number % 1 == 0) ? typeof(int) : typeof(double);
                return PopulateParam(new DataParam { Data = number, DataType = type }, component);
            }
            else
                return PopulateParam(new DataParam { Data = component.UserText, DataType = typeof(string) }, component);
        }

        /*******************************************/

        public static DataParam ToParam(this GH_NumberSlider component)
        {
            double value = (double) component.CurrentValue;
            Type type = (value % 1 == 0) ? typeof(int) : typeof(double);

            return PopulateParam(new DataParam { Data = value, DataType = type }, component);
        }

        /*******************************************/

        public static DataParam ToParam(this GH_BooleanToggle component)
        {
            return PopulateParam(new DataParam { Data = component.Value, DataType = typeof(bool) }, component);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        public static INode ToParam<T>(this GH_Param<T> component, List<object> choices = null, object selectedItem = null) where T : class, IGH_Goo
        {
            return null;
        }

        /*******************************************/

        public static INode ToParam<T>(this GH_Param<T> component) where T : class, IGH_Goo
        {
            return null;
        }

        /*******************************************/

        private static DataParam PopulateParam(this DataParam node, IGH_Param component)
        {
            node.Name = component.NickName;
            node.Description = component.Description;
            node.BHoM_Guid = component.InstanceGuid;
            node.TargetIds = component.Recipients.Select(r => r.InstanceGuid).ToList();

            return node;
        }

        /*******************************************/
    }
}
