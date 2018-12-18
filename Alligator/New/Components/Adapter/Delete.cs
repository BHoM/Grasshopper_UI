using System;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Grasshopper.Base;
using BH.UI.Grasshopper.Templates;
using BH.UI.Templates;
using BH.UI.Components;

namespace BH.UI.Grasshopper.Components
{
    public class DeleteComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new DeleteCaller();


        /*******************************************/
    }
}
