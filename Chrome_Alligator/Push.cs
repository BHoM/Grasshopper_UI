using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using CA = Chrome_Adapter;
using Grasshopper.Kernel.Types;

namespace Alligator.Mongo
{
    public class Push : GH_Component
    {
        public Push() : base("Push", "Push", "Push data to the adapter", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("A1B2BB26-6F2B-4004-88B1-08F613A72E12");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("adapter", "adapter", "adapter to send data to", GH_ParamAccess.item);
            pManager.AddGenericParameter("objects", "objects", "objects to send", GH_ParamAccess.list);
            pManager.AddTextParameter("key", "key", "key unique to that package of data", GH_ParamAccess.item);
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);

            Params.Input[1].Optional = true;
            Params.Input[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Done", "Done", "return true when the task is finished", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CA.ChromeAdapter link = null; // GHE.DataUtils.GetGenericData<CA.ChromeAdapter>(DA, 0);
            List<object> objects = new List<object>(); // GHE.DataUtils.GetGenericDataList<object>(DA, 1);
            string key = ""; // GHE.DataUtils.GetData<string>(DA, 2);
            List<string> configList = new List<string>(); // GHE.DataUtils.GetGenericDataList<string>(DA, 3);
            bool active = false; DA.GetData<bool>(4, ref active);

            DA.GetData<CA.ChromeAdapter>(0, ref link);
            DA.GetDataList<object>(1, objects);
            DA.GetData<string>(2, ref key);
            DA.GetDataList<string>(3, configList);

            for (int i = 0; i < objects.Count; i++)
                objects[i] = UnwrapObject(objects[i]);

            if (!active || objects.Count == 0)
            {
                DA.SetData(0, false);
                return;
            }

            Dictionary<string, string> config = new Dictionary<string, string>();
            foreach (string item in configList)
            {
                string[] split = item.Split(new char[] { ':' }, 2);
                if (split.Length >= 2)
                    config.Add(split[0].Trim(), split[1].Trim());
            }

            bool done = link.Push(objects, key, config);
            DA.SetData(0, done);
        }


        public static object UnwrapObject(object obj)
        {
            if (obj is Grasshopper.Kernel.Types.GH_ObjectWrapper)
                return ((Grasshopper.Kernel.Types.GH_ObjectWrapper)obj).Value;
            else if (obj is Grasshopper.Kernel.Types.GH_String)
                return ((Grasshopper.Kernel.Types.GH_String)obj).Value;
            else if (obj is Grasshopper.Kernel.Types.IGH_Goo)
            {
                try
                {
                    System.Reflection.PropertyInfo prop = obj.GetType().GetProperty("Value");
                    return prop.GetValue(obj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Grasshopper sucks, what can I do?" + e.ToString());
                }
                return obj;

            }
            else
                return obj;
        }
    }
}
