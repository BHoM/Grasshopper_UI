using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHE = Grasshopper_Engine;
using BHR = BHoM.Base.Results;
using BHI = BHoM.Structural.Interface;
using System.Windows.Forms;

namespace Alligator.Structural.Results
{
    public class LoadResults : GH_Component
    {     
        public LoadResults() : base("LoadResults", "LoadResults", "Loads results from file", "Structure", "Results")
        {
        
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{F59DFBA1-496B-40A2-8A56-3B97336E3284}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Filename", "Filename", "Filename of result server", GH_ParamAccess.item);         
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ResultServer", "ResultServer", "Result Server", GH_ParamAccess.item);

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filename = GHE.DataUtils.GetData<string>(DA, 0);

            DA.SetData(0, new BHoM.Structural.Results.StructuralResultServer(filename));                         
        }   
    }

    public class StoreResults : GH_Component
    {
        public List<CheckBox> checkBoxes;

        public StoreResults() : base("StoreResults", "StoreResults", "Store results from an application to file", "Structure", "Results")
        {
            checkBoxes = new List<CheckBox>();
            string[] OptionNames = Enum.GetNames(typeof(BHR.ResultType));
            foreach (string type in OptionNames)
            {
                CheckBox cb = new CheckBox();
                cb.Text = type;
                checkBoxes.Add(cb);
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{F59DFBA1-496B-40A2-8A56-3B97226E3284}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to extract results from", GH_ParamAccess.item);
            pManager.AddTextParameter("Filename", "Filename", "Filename of result server", GH_ParamAccess.item);
            pManager.AddTextParameter("Loadcases", "Loadcase", "Loadcases to get results", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Append", "Append", "Append results onto existing table", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Activate", "Activate", "Run the component", GH_ParamAccess.item);
            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Result database create successfully", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 4))
            {
                BHI.IResultAdapter app = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
                if (app != null)
                {
                    string filename = GHE.DataUtils.GetData<string>(DA, 1);
                    List<string> loadcases = GHE.DataUtils.GetDataList<string>(DA, 2);
                    bool append = GHE.DataUtils.GetData<bool>(DA, 3);

                    List<BHR.ResultType> types = new List<BHoM.Base.Results.ResultType>();

                    foreach (CheckBox b in checkBoxes)
                    {
                        if (b.Checked)
                        {
                            types.Add((BHR.ResultType)Enum.Parse(typeof(BHR.ResultType), b.Text));
                        }
                    }
                    DA.SetData(0, app.StoreResults(filename, types, loadcases, append));
                }
            }
        }

        public override bool AppendMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendObjectName(menu);// (menu, "Section");
            Menu_AppendEnableItem(menu);
            Menu_AppendBakeItem(menu);
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Result Selection:");
            foreach (CheckBox b in checkBoxes)
            {
                Menu_AppendCustomItem(menu, b);
            }
            Menu_AppendSeparator(menu);
            // Menu
            Menu_AppendSeparator(menu);
            Menu_AppendObjectHelp(menu);
            return true;
        }
    }
}
