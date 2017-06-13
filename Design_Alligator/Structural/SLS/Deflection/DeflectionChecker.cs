using BHoM.Base.Results;
using BHoM.Structural.Elements;
using BHoM.Structural.Interface;
using BHoM.Structural.Properties;
using BHoM.Structural.Results;
using Grasshopper.Kernel;
using StructuralDesign_Toolkit;
using StructuralDesign_Toolkit.Optimisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSD = StructuralDesign_Toolkit.SLS.Deflection;


namespace Design_Alligator.Structural.SLS.Deflection
{
    public class DeflectionChecker : GH_Component
    {

        public DeflectionChecker() : base("Beam Deflection Checker", "Beam Deflection Checker", "Check deflection-span rations for beam elements", "Structure", "Design")
        {
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("FE0E7D6A-2667-4A36-A813-43D69AA553DE");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "E", "Design elements or bars", GH_ParamAccess.list);
            pManager.AddTextParameter("Loadcases", "Loadcases", "Loadcases to design to", GH_ParamAccess.list);
            pManager.AddGenericParameter("ResultServer", "ResultServer", "Bar results", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Key", "Name of custom data key linking the bar to the result server", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Execute", "Execute", "Starts the element design", GH_ParamAccess.item);
            //pManager.AddBooleanParameter("Parallel", "Parallel", "Allows parallel execution of the sizing. Elements may be returned in a different order then inputed", GH_ParamAccess.item, false);

            Params.Input[1].Optional = true;
            Params.Input[1].AddVolatileDataList(new Grasshopper.Kernel.Data.GH_Path(0), null);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Critical values", "CritVal", "The critical values of the utilisation", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Grasshopper_Engine.DataUtils.Run(DA, 4))
            {
                List<DesignElement> elems = Grasshopper_Engine.DataUtils.GetDesignElements(DA, 0);
                List<string> loadcases = Grasshopper_Engine.DataUtils.GetDataList<string>(DA, 1);
                IResultAdapter server = Grasshopper_Engine.DataUtils.GetGenericData<IResultAdapter>(DA, 2);
                string key = Grasshopper_Engine.DataUtils.GetData<string>(DA, 3);
                //bool parallel = Grasshopper_Engine.DataUtils.GetData<bool>(DA, 5);

                List<double> critVals = new List<double>();

                SSD.DeflectionChecker defChecker = new SSD.DeflectionChecker();
                defChecker.DesignElements = elems;
                defChecker.Loadcases = loadcases;
                defChecker.ResultAdapter = server;
                defChecker.Key = key;

                //List<List<SSD.DeflectionData>> deflectionData = defChecker.CheckElements();

                //deflectionData.Select(x => x.Select(y => y.LocalDisp).ToList()).ToList().ForEach(;

                DA.SetDataList(0, defChecker.CheckElements());

            }
        }
    }
}
