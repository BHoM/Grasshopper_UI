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

using System;
using System.Linq;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.oM.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using BH.UI.Grasshopper.Global;
using BH.oM.Reflection;
using Grasshopper.Kernel.Parameters;
using BH.UI.Grasshopper.Parameters;
using BH.Engine.Reflection;
using BH.oM.Geometry;
using BH.Engine.Grasshopper;
using BH.UI.Grasshopper.Components;
using System.Collections;
using BH.Adapter;
using BH.oM.Reflection.Debugging;
using BH.UI.Base;

namespace BH.UI.Grasshopper.Templates
{
    public abstract partial class CallerComponent : GH_Component, IGH_VariableParameterComponent, IGH_InitCodeAware
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        protected virtual void OnGHParamChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (Caller == null)
                return;

            if (e?.Parameter == null || e?.ParameterIndex == -1 || e?.ParameterSide == GH_ParameterSide.Output || Caller?.InputParams.Count - 1 < e.ParameterIndex)
                return;

            // Updating Caller.InputParams based on the new Grasshopper parameter just received
            // We update the InputParams with the new type or name
            Caller.UpdateInput(e.ParameterIndex, e.Parameter.NickName, e.Parameter.Type(Caller));

            return;
        }

        /*******************************************/
    }
}


