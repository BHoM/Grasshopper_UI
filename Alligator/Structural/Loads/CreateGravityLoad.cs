using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHB = BHoM.Base;
using BHL = BHoM.Structural.Loads;
using BHG = BHoM.Geometry;

namespace Alligator.Structural.Loads
{
    public class CreateGravityLoad : GH_Component
    {

        public CreateGravityLoad() : base("Create Gravity Load", "GravityLoad", "Create a gravity load", "Structure", "Loads") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("B63BF2FA-DF96-4E89-A230-B6885BAE1944");
            }
        }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name of the load", GH_ParamAccess.item);
            pManager.AddGenericParameter("Objects", "obj", "Group of objects to apply the gravity to", GH_ParamAccess.list);
            pManager.AddGenericParameter("Load Case", "LC", "Load case for the gravity load", GH_ParamAccess.item);
            pManager.AddVectorParameter("Gravity direction", "G", "The direction to apply the gravity too. Default set to negative z", GH_ParamAccess.item, new Rhino.Geometry.Vector3d(0, 0, -1));
            pManager.AddGenericParameter("Custom Data", "CD", "Custom data for the load", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Gravity Load", "L", "The created gravity load", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = GHE.DataUtils.GetData<string>(DA, 0);
            BHB.Group<BHB.BHoMObject> objects;
            
            if(!GetGroup(DA, 1, out objects)) { return; }

            BHL.Loadcase loadCase = GHE.DataUtils.GetGenericData<BHL.Loadcase>(DA, 2);
            BHG.Vector dir = GHE.GeometryUtils.Convert(GHE.DataUtils.GetData<Rhino.Geometry.Vector3d>(DA, 3));
            Dictionary<string, object> customData = GHE.DataUtils.GetGenericData<Dictionary<string, object>>(DA, 4);

            BHL.GravityLoad load = new BHoM.Structural.Loads.GravityLoad();

            if (name != null)
                load.Name = name;

            load.Objects = objects;
            load.Loadcase = loadCase;
            load.GravityDirection = dir;

            if (customData != null)
                load.CustomData = customData;

            DA.SetData(0, load);


            
        }

        private bool GetGroup(IGH_DataAccess DA, int index, out BHB.Group<BHB.BHoMObject> group)
        {
            List<Grasshopper.Kernel.Types.GH_Goo<object>> goObjs = new List<Grasshopper.Kernel.Types.GH_Goo<object>>();


            if (!DA.GetDataList(index, goObjs))
            {
                group = null;
                return false;
            }

            if (goObjs.Count < 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Load needs a load group or nodes");
                group = null;
                return false;
            }

            if (goObjs[0].Value is BHB.IGroup)
            {
                group = new BHoM.Base.Group<BHoM.Base.BHoMObject>();
                group.Name = ((BHB.IGroup)goObjs[0].Value).Name;
                for (int i = 0; i < goObjs.Count; i++)
                {
                    group.Data.AddRange((BHB.Group<BHB.BHoMObject>)goObjs[i].Value);
                }
                return true;
            }

            group = new BHoM.Base.Group<BHoM.Base.BHoMObject>();

            for (int i = 0; i < goObjs.Count; i++)
            {
                if (goObjs[i].Value is BHB.BHoMObject)
                    group.Data.Add((BHB.BHoMObject)goObjs[i].Value);
            }

            return true;
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Bar_Gravity; }
        }
    }
}
