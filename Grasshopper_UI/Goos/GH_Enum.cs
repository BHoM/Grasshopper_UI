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

using BH.Engine.Serialiser;
using GH_IO;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using System;

namespace BH.UI.Grasshopper.Goos
{
    public class GH_Enum : GH_BHoMGoo<Enum>, GH_ISerializable
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Enum";

        public override string TypeDescription { get; } = "Defines an enum";

        public override bool IsValid { get { return Value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_Enum() : base() { }

        /***************************************************/

        public GH_Enum(Enum val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Enum { Value = Value };
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
                Value = (Enum)BH.Engine.Serialiser.Convert.FromJson(json);

            return true;
        }

        /***************************************************/

        public override bool Write(GH_IWriter writer)
        {
            if (Value != null)
                writer.SetString("Json", Value.ToJson());
            return true;
        }

        /*******************************************/
    }
}


