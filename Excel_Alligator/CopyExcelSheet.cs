using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

namespace Excel_Alligator
{
    public class CopyExcelSheet : GH_Component
    {

        public CopyExcelSheet() : base("CopyExcelSheet", "CopyExcelSheet", "Duplicates a template sheet to a number of new sheets and gives them the specified names", "Alligator", "AutoReporting")
        { }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("499BCB82-7109-49B4-8DD1-65B623FEF62C");
            }
        }

        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Go", "Go", "Go", GH_ParamAccess.item);
            pManager.AddTextParameter("newSheetNames", "newSheetNames", "List of names for the new Excel sheets", GH_ParamAccess.list);
            pManager.AddTextParameter("templateSheetName", "templateSheetName", "The name of the sheet to be copied", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("success", "success", "The operation was hopefully successfull", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //bool success = false;

            if (GHE.DataUtils.GetData<bool>(DA, 0))
            {
                 AutoReporting.AutoReporting.DuplicateTemplateSheet(GHE.DataUtils.GetDataList<string>(DA, 1), GHE.DataUtils.GetData<string>(DA, 2));
                
                DA.SetData(0, true);
            }
        }
    }
}
