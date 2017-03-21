using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHE = Grasshopper_Engine;
using BHR = BHoM.Base.Results;
using BHI = BHoM.Structural.Interface;
using BHD = BHoM.Databases;
using System.Windows.Forms;

namespace Alligator.Structural.Results
{
    public class PushResultsToDataBase : GH_Component
    {
        public List<CheckBox> checkBoxes;

        public PushResultsToDataBase() : base("PushToDb", "PushToDb", "Store results from an application to a database", "Structure", "Results")
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
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_StoreResults; }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("7A58BD14-836F-40DB-8C0E-ED342566B640");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to extract results from", GH_ParamAccess.item);
            pManager.AddGenericParameter("DataBase", "Db", "Database to push to", GH_ParamAccess.item);
            pManager.AddTextParameter("Loadcases", "Loadcase", "Loadcases to get results", GH_ParamAccess.list);
            pManager.AddTextParameter("Key", "Key", "Key", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Append", "Append", "Append results onto existing table", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Activate", "Activate", "Run the component", GH_ParamAccess.item);
            Params.Input[2].Optional = true;
            Params.Input[4].Optional = true;
            Params.Input[5].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Result database create successfully", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 5))
            {
                BHI.IResultAdapter app = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
                BHD.IDatabaseAdapter db = GHE.DataUtils.GetGenericData<BHD.IDatabaseAdapter>(DA, 1);
                if (app != null && db != null)
                {
                    List<string> loadcases = GHE.DataUtils.GetDataList<string>(DA, 2);
                    string key = GHE.DataUtils.GetData<string>(DA, 3);
                    bool append = GHE.DataUtils.GetData<bool>(DA, 4);

                    List<BHR.ResultType> types = new List<BHoM.Base.Results.ResultType>();

                    foreach (CheckBox b in checkBoxes)
                    {
                        if (b.Checked)
                        {
                            types.Add((BHR.ResultType)Enum.Parse(typeof(BHR.ResultType), b.Text));
                        }
                    }
                    DA.SetData(0, app.PushToDataBase(db, types, loadcases, key, append));
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
