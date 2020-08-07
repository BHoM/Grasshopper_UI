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
using Grasshopper.Kernel.Parameters.Hints;

namespace BH.UI.Grasshopper.Templates
{
    public abstract partial class CallerComponent : GH_Component, IGH_VariableParameterComponent, IGH_InitCodeAware
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public virtual bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input)
                return Caller.CanAddInput();
            else
                return Caller.CanAddOutput();
        }

        /*************************************/

        public virtual bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input)
            {
                if (Params.Input.Count <= index)
                    return false;
                else
                    return Caller.CanRemoveInput(Params.Input[index].NickName);
            }
            else
            {
                if (Params.Output.Count <= index)
                    return false;
                else
                    return Caller.CanRemoveOutput(Params.Output[index].NickName);
            }  
        }

        /*************************************/

        public virtual IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            string name = GH_ComponentParamServer.InventUniqueNickname("xyzuvw", this.Params.Input);
            Param_ScriptVariable param = new Param_ScriptVariable
            {
                NickName = name,
                Name = name,
                TypeHint = new GH_NullHint()
            };

            // Updating the caller with the parameter that Grasshopper just added
            Caller.AddInput(index, param.NickName, param.Type());
            return param;
        }

        /*******************************************/

        public virtual bool DestroyParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input)
            {
                if (Params.Input.Count <= index)
                    return true;
                else
                    Caller.RemoveInput(Params.Input[index].NickName);
            }
            else
            {
                if (Params.Output.Count <= index)
                    return true;
                else
                    Caller.RemoveOutput(Params.Output[index].NickName);
            }
                return true;
        }

        /*******************************************/

        public virtual void VariableParameterMaintenance()
        {
            foreach (IGH_Param param in Params.Input)
            {
                Param_ScriptVariable paramScript = param as Param_ScriptVariable;
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

