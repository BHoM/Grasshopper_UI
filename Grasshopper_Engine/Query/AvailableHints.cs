/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.UI.Grasshopper.Objects.Hints;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Parameters.Hints;
using System.Collections.Generic;

namespace BH.Engine.Grasshopper
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public static List<IGH_TypeHint> AvailableHints = new List<IGH_TypeHint>()
        {
            new GH_NullHint(),
            new GH_HintSeparator(),
            new BHoMObjectHint(),
            new IGeometryHint(),
            new DictionaryHint(),
            new EnumHint(),
            new TypeHint(),
            new GH_HintSeparator(),
            new GH_BooleanHint_CS(),
            new GH_IntegerHint_CS(),
            new GH_DoubleHint_CS(),
            new GH_StringHint_CS(),
            new GH_HintSeparator(),
            new GH_DateTimeHint(),
            new GH_ColorHint(),
            new GH_GuidHint()
        };

        /***************************************************/
    }
}

