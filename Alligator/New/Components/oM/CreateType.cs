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
        /**** Constructors                      ****/
        /*******************************************/

        public CreateTypeComponent() : base()
        {
            Caller.ItemSelected += DynamicCaller_ItemSelected;
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void DynamicCaller_ItemSelected(object sender, object e)
        {
            Type type = e as Type;

            if (type != null)
                Message = type.ToText();
        }


        /*******************************************/
    }
}
