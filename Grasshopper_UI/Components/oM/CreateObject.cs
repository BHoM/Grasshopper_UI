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

using System;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Grasshopper.Templates;
using BH.UI.Base;
using BH.UI.Base.Components;
using System.Reflection;
using BH.oM.UI;
using System.Linq;

namespace BH.UI.Grasshopper.Components
{
    public class CreateObjectComponent : CallerComponent, IGH_VariableParameterComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateObjectCaller();


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input && Caller.SelectedItem is Type;
        }

        /*******************************************/

        public override bool DestroyParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output)
                return true;

            if (Params.Input.Count <= index)
                return true;

            // Updating the caller with the parameter that Grasshopper just removed
            CreateObjectCaller caller = Caller as CreateObjectCaller;
            if (caller != null)
                caller.RemoveInput(Params.Input[index].NickName);
            return true;
        }


        /*******************************************/
    }
}

