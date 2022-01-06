/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.UI.Grasshopper.Goos;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Grasshopper.Hints
{
    public class BHoMObjectHint : IGH_TypeHint
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Guid HintID { get; } = new Guid("0977C35E-92DD-4933-8835-8B2C8A37C8CF"); 

        public string TypeName { get; } = "BH.oM.Base.BHoMObject"; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public bool Cast(object data, out object target)
        {
            GH_BHoMObject obj = new GH_BHoMObject() { Value = null };
            obj.CastFrom(data);
            if (obj.Value == null)
                target = data;
            else
                target = obj.Value;
            return true;
        }

        /*******************************************/
    }
}



