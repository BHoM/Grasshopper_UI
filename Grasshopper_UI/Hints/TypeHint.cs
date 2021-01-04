/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using Grasshopper.Kernel.Parameters;
using BH.UI.Grasshopper.Goos;

namespace BH.UI.Grasshopper.Hints
{
    public class TypeHint : IGH_TypeHint
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Guid HintID { get; } = new Guid("8ECF16E7-F71B-4813-AD63-C4AECC246A26");

        public string TypeName { get; } = "System.Type";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public bool Cast(object data, out object target)
        {
            GH_Type type = new GH_Type() { Value = null };
            type.CastFrom(data);
            if (type.Value == null)
                target = data;
            else
                target = type.Value;
            return true;
        }

        /*******************************************/
    }
}


