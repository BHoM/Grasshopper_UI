using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class Push : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Base.Properties.Resources.Push; 

        public override Guid ComponentGuid { get; } = new Guid("040CEC18-C6E1-443B-B816-72B100304536"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Push() : base("Push", "Push", "Push objects to the external software", "Alligator", " Adapter") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddParameter(new IObjectParameter(), "Objects", "Objects", "Objects to push", GH_ParamAccess.list);
            pManager.AddTextParameter("Tag", "Tag", "Tag to apply to the objects being pushed", GH_ParamAccess.item,"");
            pManager.AddParameter(new BHoMObjectParameter(), "Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the push", GH_ParamAccess.item);
            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Success", GH_ParamAccess.item);
            pManager.AddParameter(new IObjectParameter(), "Objects", "Objects", "Pushed objects", GH_ParamAccess.list);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            BHoMAdapter adapter = null; DA.GetData(0, ref adapter);
            List<IObject> objects = new List<IObject>(); DA.GetDataList(1, objects);
            string tag = ""; DA.GetData(2, ref tag);
            CustomObject config = new CustomObject(); DA.GetData(3, ref config);
            bool active = false; DA.GetData(4, ref active);
            bool success = false;

            if (active)
            {
                List<IObject> returnObjects = adapter.Push(objects, tag, config.CustomData);
                success = returnObjects.Count == objects.Count;
                System.Threading.Thread.Sleep(200);
                DA.SetDataList(1, returnObjects);
            }
                
            DA.SetData(0, success);

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*******************************************/
    }
}
