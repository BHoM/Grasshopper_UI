using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using MA = Mongo_Adapter;
using GHE = Grasshopper_Engine;

namespace Alligator.Mongo
{
    public class FromMongoParallelOrdered : GH_Component
    {
        public FromMongoParallelOrdered() : base("FromMongoParallelOrdered", "FromMongoParallelOrdered", "Get BHoM objects from a Mongo database", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("F53906B2-E5B6-4EB8-A303-61E88E225650");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Mongo_Alligator.Properties.Resources.BHoM_Mongo_From; }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Mongo link", "link", "collection to get the data from", GH_ParamAccess.item);
            pManager.AddTextParameter("query", "query", "query string", GH_ParamAccess.list);
            pManager.AddBooleanParameter("toJson", "toJson", "output as json instead of objects", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("result", "result", "result from the query", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoLink link = GHE.DataUtils.GetGenericData<MA.MongoLink>(DA, 0);
            List<string> query = GHE.DataUtils.GetDataList<string>(DA, 1);
            bool toJson = GHE.DataUtils.GetData<bool>(DA, 2);
            bool active = false; DA.GetData<bool>(3, ref active);

            if (active)
                m_LastResult = CheckAndGetTree(link.QueryParallelOrdered(query, toJson));

            DA.SetDataTree(0, m_LastResult);
            
        }

        public Grasshopper.DataTree<object> CheckAndGetTree(List<object> list)
        {
            bool isTree = true;

            foreach (var item in list)
            {
                if (!(item is IList))
                {
                    isTree = false;
                    break;
                }
            }

            Grasshopper.DataTree<object> tree = new Grasshopper.DataTree<object>();

            if (!isTree)
            {
                tree.AddRange(list);
            }
            else
            {
                int n = 0;
                Grasshopper.Kernel.Data.GH_Path path;
                foreach (var innerList in list)
                {
                    path = new Grasshopper.Kernel.Data.GH_Path(n);
                    tree.AddRange(innerList as IEnumerable<object>, path);
                    n++;
                }

            }

            return tree;
        }


        private Grasshopper.DataTree<object> m_LastResult = new Grasshopper.DataTree<object>();
    }
}
