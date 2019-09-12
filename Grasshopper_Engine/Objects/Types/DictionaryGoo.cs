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

using BH.oM.Base;
using Grasshopper.Kernel.Types;
using System.Collections;

namespace BH.Engine.Grasshopper.Objects
{
    public class DictionaryGoo : BHGoo<IDictionary>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Dictionary";

        public override string TypeDescription { get; } = "Defines an Dictionary";

        public override bool IsValid { get { return Value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public DictionaryGoo() : base() { }

        /***************************************************/

        public DictionaryGoo(IDictionary val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new DictionaryGoo { Value = Value };
        }

        /*******************************************/

        public override bool CastFrom(object source)
        {
            if (source is CustomObject)
                source = ((CustomObject)source).CustomData;
            else if (source is IGH_Goo)
                source = ((IGH_Goo)source).ScriptVariable();

            return base.CastFrom(source);
        }

        /*******************************************/
    }
}
