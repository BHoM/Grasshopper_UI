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

using BH.Engine.Base;
using BH.Engine.Serialiser;
using GH_IO;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using System;

namespace BH.UI.Grasshopper.Goos
{
    public class GH_Type : GH_BHoMGoo<Type>, GH_ISerializable
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Type";

        public override string TypeDescription { get; } = "Defines an object Type";

        public override bool IsValid { get { return m_value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_Type() : base() { }

        /***************************************************/

        public GH_Type(Type val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Type { Value = Value };
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            try
            {
                if (source == null) { return false; }
                else if (source is string)
                    this.Value = BH.Engine.Base.Create.Type(source as string);
                else if (source is GH_String)
                    this.Value = BH.Engine.Base.Create.Type(((GH_String)source).Value);
                else if (source.GetType() == typeof(GH_Goo<Type>))
                    this.Value = (Type)source;
                else if (source is GH_Variable)
                    this.Value = (Type)((GH_Variable)source).Value;
                else
                    this.Value = (Type)source;
            }
            catch
            {
                string message = $"Impossible to convert {source.GetType().IToText()} into System.Type. Check the description of each input for more details on the type of object that need to be provided";
                BH.Engine.Base.Compute.RecordError(message);
                return false;
            }
            
            return true;
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
                Value = (Type)BH.Engine.Serialiser.Convert.FromJson(json);

            return true;
        }

        /***************************************************/

        public override bool Write(GH_IWriter writer)
        {
            if (Value != null)
                writer.SetString("Json", Value.ToJson());
            return true;
        }

        /***************************************************/
    }
}



