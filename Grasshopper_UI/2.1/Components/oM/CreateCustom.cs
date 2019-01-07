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

namespace BH.UI.Grasshopper.Components
{
    public class CreateCustomComponent : CallerComponent, IGH_VariableParameterComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateCustomCaller();


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            // Let this component be in charge of storing the inputs 
            // (Deserialisation happens after RegisterInputParams so the caller has not had a chance to retreive its input list yet)

            if (Caller is CreateCustomCaller caller)
            {
                caller.ItemSelected += (sender, e) => base.RegisterInputParams(pManager);
                caller.SetInputs(Params.Input.Select(x => x.NickName).ToList());
            }
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
            return new Param_ScriptVariable
            {
                NickName = GH_ComponentParamServer.InventUniqueNickname("xyzuvw", this.Params.Input)
            };
        }

        /*******************************************/

        public override void VariableParameterMaintenance()
        {
            CreateCustomCaller caller = Caller as CreateCustomCaller;
            Params.Input.RemoveAll(p => p.Name == "CustomData");

            List<string> nicknames = new List<string>();
            foreach(IGH_Param param in Params.Input)
            {
                if (param is Param_ScriptVariable paramScriptVariable)
                {
                    paramScriptVariable.ShowHints = true;
                    paramScriptVariable.Hints = Engine.Grasshopper.Query.AvailableHints;
                    nicknames.Add(paramScriptVariable.NickName);
                }
            }
            caller.SetInputs(nicknames);
        }

        /*******************************************/

        public override bool Read(GH_IReader reader)
        {
            if (!base.Read(reader) || !Params.Read(reader))
                return false;

            return true;
        }

        /*******************************************/
    }
}
