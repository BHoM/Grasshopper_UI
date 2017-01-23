using BHoM.Structural.Elements;
using BH = BHoM.Geometry;
using BHG = BHoM.Geometry;
using R = Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Unreal
{
    public class UnrealProject : GH_Component
    {
        public UnrealProject() : base("UnrealProject", "UnrealProject", "Combine Unreal Project Messages", "Alligator", "Unreal") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("d4386097-9438-4e09-9f0d-d6283cd37a0b");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Message List", "Bars", "List of project Messages", GH_ParamAccess.list);
            pManager.AddTextParameter("Project Name", "Name", "Project Name", GH_ParamAccess.item);
            pManager.AddTextParameter("Save slot index", "SaveSlot", "Save slot index", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("ProjectMessage", "ProjectMessage", "Complete Project Message", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> Messages = DataUtils.GetDataList<string>(DA, 0);

            string json = "[[[[[" + DataUtils.GetData<string>(DA, 1) + "]]]],";

            json += "[[[[" + DataUtils.GetData<string>(DA, 2) + "]]]],";

            for (int i = 0; i < Messages.Count ; i++)
            {
                json += "[" + Messages[i] + "],";
            }
            json = json.Trim(',') + "]";
            DA.SetData(0, json);
        }
    }
}
