using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class Move : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Move;

        public override Guid ComponentGuid { get; } = new Guid("A964110F-C8F5-4946-BAE8-D829CF91D7CA");

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Move() : base("Move", "Move", "Move objects between two external softwares", "Alligator", " Adapter") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Source", "Source", "Adapter the data is pulled from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Target", "Target", "Adapter the data is pushed to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Query", "Query", "BHoM Query", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the pull", GH_ParamAccess.item);
            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Confirms if the operation was successful", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMAdapter from = null; DA.GetData(0, ref from);
            BHoMAdapter to = null; DA.GetData(1, ref to);
            IQuery query = null; DA.GetData(2, ref query);
            CustomObject config = new CustomObject(); DA.GetData(3, ref config);
            bool active = false; DA.GetData(4, ref active);

            if (!active) return;

            if (query == null)
                query = new FilterQuery();

            bool success = from.PullTo(to, query, config.CustomData);
            DA.SetData(0, success);
        }

        /*******************************************/
    }
}
