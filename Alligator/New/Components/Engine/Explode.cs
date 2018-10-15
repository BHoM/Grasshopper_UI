using System;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Templates;
using BH.UI.Templates;
using BH.UI.Components;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Alligator.Components
{
    public class ExplodeComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new ExplodeCaller();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ExplodeComponent() : base()
        {
            Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();

            if (Params.Input.Count == 1 && Caller.OutputParams.Count != Params.Output.Count)
            {
                Params.Input[0].CollectData();
                List<object> data = Params.Input[0].VolatileData.AllData(true).Select(x => x.ScriptVariable()).ToList();
                ExplodeCaller caller = Caller as ExplodeCaller;
                caller.CollectOutputTypes(data);
            }
        }

        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            if (Params.Input.Count == 0)
                base.RegisterInputParams(pManager);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            // Update the output params based on input data
            Params.Input[0].CollectData();
            List<object> data = Params.Input[0].VolatileData.AllData(true).Select(x => x.ScriptVariable()).ToList();
            ExplodeCaller caller = Caller as ExplodeCaller;
            caller.CollectOutputTypes(data);

            // Forces the component to update
            IGH_Attributes backup = m_attributes;
            PostConstructor();
            this.m_attributes = backup;

            this.OnAttributesChanged();
            ExpireSolution(true);
        }

        /*******************************************/
    }
}
