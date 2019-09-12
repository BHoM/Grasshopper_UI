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

using GH_IO;
using Grasshopper.Kernel.Types;
using System;

namespace BH.Engine.Grasshopper.Objects
{
    public class TypeGoo : BHGoo<Type>
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

        public TypeGoo() : base() { }

        /***************************************************/

        public TypeGoo(Type val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool CastFrom(object source)
        {
            if (source == null)
                return false;
            else if (source is string)
                try { source = BH.Engine.Reflection.Create.Type(source as string); } catch { }
            else if (source is GH_String)
                try { source = BH.Engine.Reflection.Create.Type(((GH_String)source).Value); } catch { }
            else if (source is Type)
                source = (Type)source;
            else if (source is IGH_Goo)
                source = ((IGH_Goo)source).ScriptVariable().GetType();
            else
                source = source.GetType();

            return base.CastFrom(source);
        }

        /***************************************************/
    }
}
