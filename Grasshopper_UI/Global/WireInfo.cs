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

using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.UI.Grasshopper.Components;
using BH.UI.Grasshopper.Templates;
using BH.UI.Base.Global;
using BH.UI.Base;
using Grasshopper.GUI.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GH = Grasshopper;
using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;

namespace BH.UI.Grasshopper.Global
{
    public class WireInfo
    {
        /*******************************************/
        /**** Public Properties                 ****/
        /*******************************************/

        public GH_WireInteraction Wire { get; set; } = null;

        public IGH_Param Source { get; set; } = null;

        public Type SourceType { get; set; } = null;

        public bool IsInput { get; set; } = false;

        public HashSet<string> Tags { get; set; } = new HashSet<string>();

        /*******************************************/
    }
}


