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

using Grasshopper.Kernel;
using BH.UI.Grasshopper.Templates;
using BH.UI.Templates;
using BH.UI.Components;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Grasshopper.Components
{
    public class SetPropertyComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new SetPropertyCaller();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public SetPropertyComponent() : base()
        {
            this.Params.ParameterChanged += OnGrasshopperUpdates;
            if (Params.Input.Count > 2)
            {
                Param_ScriptVariable paramScript = Params.Input[2] as Param_ScriptVariable;
                if (paramScript != null)
                {
                    paramScript.ShowHints = true;
                    paramScript.Hints = Engine.Grasshopper.Query.AvailableHints;
                    paramScript.AllowTreeAccess = true;
                }
            }
        }

        /*******************************************/
    }
}

