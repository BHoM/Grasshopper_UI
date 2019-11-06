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

using System;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Grasshopper.Templates;
using BH.UI.Templates;
using BH.UI.Components;
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
        /**** Constructors                      ****/
        /*******************************************/

        public CreateObjectComponent() : base()
        {
            CreateObjectCaller caller = Caller as CreateObjectCaller;
            if (caller != null)
                caller.InputToggled += Caller_InputToggled;
        }


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
        /**** Private Methods                   ****/
        /*******************************************/

        private void Caller_InputToggled(object sender, Tuple<ParamInfo, bool> e)
        {
            if (e.Item2)
                Params.RegisterInputParam(ToGH_Param(e.Item1));
            else
            {
                IGH_Param param = Params.Input.FirstOrDefault(x => x.Name == e.Item1.Name);
                if (param != null)
                    Params.UnregisterInputParameter(param);
            }                

            this.Params.OnParametersChanged(); // and ask to update the layout with OnParametersChanged()
            this.ExpireSolution(true);
        }


        /*******************************************/
    }
}
