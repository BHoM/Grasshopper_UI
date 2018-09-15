using System;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Templates;
using BH.UI.Templates;
using BH.UI.Components;

namespace BH.UI.Alligator.Components
{
    public class CreateQueryComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override Caller Caller { get; } = new CreateQueryCaller();


        /*******************************************/
    }
}
