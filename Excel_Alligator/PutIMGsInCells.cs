using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

namespace Excel_Alligator
{
    public class PutIMGsInCells : GH_Component
    {

        public PutIMGsInCells() : base("PutIMGsInCells", "PutIMGsInCells", "Puts images in the cells with the right keyword and the sheets with the right name.Images must be pngs and their name should have the format *****_SheetName_CellKeyword. the images will be put in the corresponding sheet and cell", "Alligator", "AutoReporting")
        { }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("B64D4391-9AAB-459C-A2B3-EE3D8E7C888A");
            }
        }

        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Go", "Go", "Go", GH_ParamAccess.item);
            pManager.AddTextParameter("cellKeys", "cellKeys", "the keywords for the cell where the images are to be blaced", GH_ParamAccess.list);
            pManager.AddTextParameter("Sheetnames", "SheetNames", "The names of the sheets where images are to be placed", GH_ParamAccess.list);
            pManager.AddTextParameter("Folder", "Folder", "The folder where the images can be found, their name needs to be of the format *_Sheetname_cellkey.png", GH_ParamAccess.item);
        
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("out", "out", "output message", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //bool success = false;
            string outmessage = "press Go!! But make sure you have the right Excel document active";

            if (GHE.DataUtils.GetData<bool>(DA, 0))
            {

                 outmessage = AutoReporting.AutoReporting.InsertImageInCell(GHE.DataUtils.GetDataList<string>(DA, 1), GHE.DataUtils.GetDataList<string>(DA, 2), GHE.DataUtils.GetData<string>(DA, 3));
            }
                DA.SetData(0, outmessage);
            
        }
    }
}
