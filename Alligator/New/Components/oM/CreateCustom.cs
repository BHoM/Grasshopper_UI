using System;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Templates;
using BH.UI.Templates;
using BH.UI.Components;
using System.Linq;
using BH.Engine.Grasshopper;
using System.Collections.Generic;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.Components
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
            caller.SetInputs(Params.Input.Select(x => x.NickName).ToList());
        }

        /*******************************************/
    }
}
