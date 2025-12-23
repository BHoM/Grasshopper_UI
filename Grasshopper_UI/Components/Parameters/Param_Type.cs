/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using BH.UI.Grasshopper.Properties;
using BH.UI.Grasshopper.Templates;
using BH.UI.Grasshopper.Goos;
using System;

namespace BH.UI.Grasshopper.Parameters
{
    public class Param_Type : BHoMParam<GH_Type>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.Type_Param;

        public override Guid ComponentGuid { get; } = new Guid("AA7DDCDC-2789-4A23-88AD-E1E4CD84FB37");

        public override string TypeName { get; } = "Type";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_Type() : base("Object Type", "Type", "Represents the type of an object", "Params", "Primitive")
        {
        }

        /*******************************************/
    }
}







