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
    public class Execute : GH_Component
    {
        public Execute() : base("Execute", "Execute", "Execute command with the adapter", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("875154A3-48FE-41E9-B804-2C676CE25BD3");
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
            pManager.AddGenericParameter("adapter", "adapter", "adapter to get data from", GH_ParamAccess.item);
            pManager.AddTextParameter("command", "command", "command", GH_ParamAccess.item);
            pManager.AddTextParameter("params", "params", "parameters", GH_ParamAccess.list);
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Done", "Done", "return true when the task is finished", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CA.ChromeAdapter link = null; // GHE.DataUtils.GetGenericData<CA.ChromeAdapter>(DA, 0);
            string command = ""; // GHE.DataUtils.GetData<string>(DA, 1);
            List<string> configList = new List<string>(); // GHE.DataUtils.GetGenericDataList<string>(DA, 3);
            List<string> parameters = new List<string>(); // GHE.DataUtils.GetGenericDataList<string>(DA, 3);
            bool active = false; DA.GetData<bool>(4, ref active);

            DA.GetData<CA.ChromeAdapter>(0, ref link);
            DA.GetData<string>(1, ref command);
            DA.GetDataList<string>(2, parameters);
            DA.GetDataList<string>(3, configList);

            if (!active)
            {
                DA.SetData(0, false);
                return;
            }

            Dictionary<string, string> config = new Dictionary<string, string>();
            foreach (string item in configList)
            {
                string[] split = item.Split(':');
                if (split.Length == 2)
                    config.Add(split[0].Trim(), split[1].Trim());
            }

            bool done = link.Execute(command, parameters, config);
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
