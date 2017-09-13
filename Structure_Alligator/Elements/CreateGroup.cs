using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Grasshopper.Kernel;
using BHB = BH.oM.Base;
using BH.Engine.Grasshopper;
using Grasshopper.Kernel.Types;

namespace BH.UI.Alligator.Structural.Elements
{
    public class CreateGroup : GH_Component
    {

        public CreateGroup() : base("Create Group", "Group", "Creat a group of items", "Structure", "Elements")
        {

        }      
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Group; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("32627B02-E84E-45EB-BCAF-0B199177A301");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "Data to group", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "N", "Name of the group", GH_ParamAccess.item);
            pManager.AddGenericParameter("Custom Data", "CU", "Custom data of the group", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Group", "G", "The created group", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";

            //List<GH_Goo<object>> objects = new List<GH_Goo<object>>();

            List<BHB.BHoMObject> objects = DataUtils.GetDataList<BHB.BHoMObject>(DA, 0);

            if (objects == null)
                return;

            //if(!DA.GetDataList(0, objects)) { return; }
            if(!DA.GetData(1, ref name)) { return; }

            if (objects.Count < 1)
                return;

            Type t = objects[0].GetType();

            bool sameType = true;

            for (int i = 1; i < objects.Count; i++)
            {
                if (objects[i].GetType() != t)
                {
                    sameType = false;
                    break;
                }

            }

            Dictionary<string, object> customData = DataUtils.GetData<Dictionary<string, object>>(DA, 2);

            if (sameType)
            {
                //If all of the provided objects is of the same type, a Group<T> where T is 
                //the object type is created using reflection

                BHB.BHoMGroup group = new BHB.BHoMGroup(objects);
                group.Name = name;
                group.CustomData = customData;

                DA.SetData(0, group);

            }
            else
            {

                BHB.BHoMGroup group = new BHB.BHoMGroup(objects);

                group.Name = name;

                if (customData != null)
                {
                    foreach (KeyValuePair<string, object> item in customData)
                    {
                        group.CustomData[item.Key] = item.Value;
                    }
                }

                DA.SetData(0, group);
            }


        }
    }
}
