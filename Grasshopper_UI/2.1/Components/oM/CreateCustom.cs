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

using Grasshopper.Kernel;
using BH.UI.Grasshopper.Templates;
using BH.UI.Templates;
using BH.UI.Components;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel.Parameters;
using GH_IO.Serialization;
using System.Runtime.CompilerServices;
using System;

namespace BH.UI.Grasshopper.Components
{
    public class CreateCustomComponent : CallerComponent, IGH_VariableParameterComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateCustomCaller();

        public CreateCustomComponent() : base()
        {
            Params.ParameterChanged += (sender, e) => RefreshComponent();
        }

        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            // Let this component be in charge of storing the inputs 
            // (RegisterInputParams(pManager) happens before Read(reader), so the caller does not know the right input list yet)
            SyncParamsFromGH();
        }

        /*******************************************/

        public override bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input;
        }

        /*******************************************/

        public override bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input;
        }

        /*******************************************/

        public override IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            Param_ScriptVariable param = new Param_ScriptVariable
            {
                NickName = GH_ComponentParamServer.InventUniqueNickname("xyzuvw", this.Params.Input)
            };
            return param;
        }

        /*******************************************/

        public override void VariableParameterMaintenance()
        {
            SyncParamsFromGH();
        }

        /*******************************************/

        private void SyncParamsFromGH()
        {
            if (Caller is CreateCustomCaller caller)
            {
                List<string> nicknames = new List<string>();
                List<Type> types = new List<Type>();
                foreach (IGH_Param param in Params.Input)
                {
                    string name = param.NickName;
                    if (param is Param_ScriptVariable paramScript)
                    {
                        paramScript.ShowHints = true;
                        paramScript.Hints = Engine.Grasshopper.Query.AvailableHints;
                        if (paramScript.TypeHint != null)
                        {
                            types.Add(Engine.Grasshopper.Query.Type(paramScript.TypeHint));
                        }
                        else
                            types.Add(typeof(object));
                    }
                    else
                    {
                        types.Add(param.Type);
                    }
                    nicknames.Add(name);
                }

                caller.SetInputs(nicknames, types);
            }
        }

        /*******************************************/
    }
}
