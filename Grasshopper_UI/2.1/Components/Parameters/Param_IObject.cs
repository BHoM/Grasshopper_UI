/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
using System;

namespace BH.UI.Grasshopper.Parameters
{
    public class Param_IObject : BHoMParam<Engine.Grasshopper.Objects.GH_IObject>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } =  Resources.IObject_Param;

        public override Guid ComponentGuid { get; } = new Guid("FFE324E7-1FC0-4818-9FCB-43A0202CC974");

        public override string TypeName { get; } = "IObject";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_IObject() : base("BH IObject", "IObject", "Represents a collection of generic BH IObjects", "Params", "Primitive")
        {
        }

        /*******************************************/
    }
}
