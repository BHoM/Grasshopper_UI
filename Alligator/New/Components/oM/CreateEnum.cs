using System;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Templates;
using BH.UI.Templates;
using BH.UI.Components;
using BH.Engine.Reflection;
using Grasshopper.Kernel.Special;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace BH.UI.Alligator.Components
{
    public class CreateEnumComponent : CallerValueList
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MultiChoiceCaller Caller { get; } = new CreateEnumCaller();

        /*******************************************/
    }
}
