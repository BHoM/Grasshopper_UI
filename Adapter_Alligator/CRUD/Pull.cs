using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class Pull : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Pull; 

        public override Guid ComponentGuid { get; } = new Guid("BA3D716D-3044-4795-AC81-0FECC80781E3"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Pull() : base("Pull", "Pull", "Pull objects from the external software", "Alligator", " Adapter") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddGenericParameter("Query", "Query", "BHoM Query", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the pull", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "Objects", "Objects obtained from the query", GH_ParamAccess.list);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMAdapter adapter = null; DA.GetData(0, ref adapter);
            IQuery query = null; DA.GetData(1, ref query);
            CustomObject config = new CustomObject(); DA.GetData(2, ref config);
            bool active = false; DA.GetData(3, ref active);

            if (!active) return;

            if (query == null)
                query = new FilterQuery();

            Guid id = adapter.BHoM_Guid;
            if (id != m_AdapterId)
            {
                m_AdapterId = id;
                adapter.DataUpdated += Adapter_DataUpdated;
            }

            IEnumerable<object> objects = adapter.Pull(query, config.CustomData);
            DA.SetDataList(0, objects);
        }

        /*******************************************/

        private void Adapter_DataUpdated(object sender, EventArgs e)
        {
            ExpireSolution(true);
        }

        /*******************************************/

        private Guid m_AdapterId;
    }
}
