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

using BH.UI.Grasshopper.Goos;
using BH.UI.Grasshopper.Properties;
using BH.UI.Grasshopper.Templates;
using System;

namespace BH.UI.Grasshopper.Parameters
{
    public class Param_Dictionary : BHoMParam<GH_Dictionary>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.Dictionary_Param;

        public override Guid ComponentGuid { get; } = new Guid("82AA94FD-F2D9-4DBD-9425-F4C9EA8A1C37");

        public override string TypeName { get; } = "Dictionary";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_Dictionary(): base("Dictionary", "Dictionary", "Represents an Dictionary", "Params", "Primitive")
        {
        }

        /*******************************************/
    }
}





