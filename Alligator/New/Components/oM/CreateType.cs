using System;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Templates;
using BH.UI.Templates;
using BH.UI.Components;
using BH.Engine.Reflection.Convert;

namespace BH.UI.Alligator.Components
{
    public class CreateTypeComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateTypeCaller();


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        public override void RefreshComponent()
        {
            base.RefreshComponent();

            Type type = Caller.SelectedItem as Type;
            if (type != null)
                Message = type.ToText();
        }


        /*******************************************/
    }
}
