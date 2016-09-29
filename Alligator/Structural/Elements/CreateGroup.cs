using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Grasshopper.Kernel;
using BHB = BHoM.Base;
using Grasshopper_Engine;
using Grasshopper.Kernel.Types;

namespace Alligator.Structural.Elements
{
    public class CreateGroup : GH_Component
    {

        public CreateGroup() : base("Create Group", "Group", "Creat a group of items", "Structure", "Elements")
        {

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

                var genericGroupType = typeof(BHB.Group<>);
                var specificGroupType = genericGroupType.MakeGenericType(t);
                var group = Activator.CreateInstance(specificGroupType);
                PropertyInfo propName = specificGroupType.GetProperty("Name");
                propName.SetValue(group, name);

                PropertyInfo propData = specificGroupType.GetProperty("Data");

                var data = propData.GetValue(group);
                MethodInfo metInfoDataAdd = data.GetType().GetMethod("Add");

                for (int i = 0; i < objects.Count; i++)
                {
                    metInfoDataAdd.Invoke(data, new object[] { objects[i] });
                }

                PropertyInfo propCustomData = specificGroupType.GetProperty("CustomData");

                var custData = propCustomData.GetValue(group);

                MethodInfo customDataAdd = custData.GetType().GetMethod("Add");

                if (customData != null)
                {
                    foreach (KeyValuePair<string, object> item in customData)
                    {
                        customDataAdd.Invoke(custData, new object[] { item.Key, (object)item.Value });
                    }
                }

                DA.SetData(0, group);

            }
            else
            {

                BHB.Group<BHB.BHoMObject> group = new BHB.Group<BHB.BHoMObject>(objects);

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
