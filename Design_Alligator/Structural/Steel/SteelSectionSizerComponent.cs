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


namespace Design_Alligator.Structural.Steel
{
    public class SteelSectionSizerComponent : Grasshopper_Engine.Components.BHoMBaseComponent<BHoM.Base.BHoMObject>
    {

        private List<DesignElement> m_desElems;
        private List<SteelUtilisation> m_utils;
        private List<double> m_critVals;
        private List<double> m_tonnageDifference;
        public SteelSectionSizerComponent() : base("Steel Section Sizer", "SteelSizer", "Check a set of design element for a set of section properties until a good match is found", "Structure", "Design")
        {
            m_desElems = new List<DesignElement>();
            m_utils = new List<SteelUtilisation>();
            m_critVals = new List<double>();
            m_tonnageDifference = new List<double>();
            AppendEnumOptions("Sizing mode", typeof(SizerMode));
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("7FEABF2D-56D6-4538-A2F4-E61FE8292FE9");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "E", "Design elements or bars", GH_ParamAccess.list);
            pManager.AddTextParameter("Loadcases", "Loadcases", "Loadcases to design to", GH_ParamAccess.list);
            pManager.AddGenericParameter("ResultServer", "ResultServer", "Bar results", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Key", "Name of custom data key linking the bar to the result server", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Allow End Offset", "AllowOffset", "Allows and end offset check for over utilised members", GH_ParamAccess.item, false);
            pManager.AddGenericParameter("SectionProperties", "SecProps", "The allowed section properties to check for", GH_ParamAccess.list);
            pManager.AddNumberParameter("MinUtil", "MinUtil", "The minimal allowed utilisation", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("MaxUtil", "MaxUtil", "The maximal allowed utilisation", GH_ParamAccess.item, 1);
            pManager.AddBooleanParameter("Execute", "Execute", "Starts the element design", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Parallel", "Parallel", "Allows parallel execution of the sizing. Elements may be returned in a different order then inputed", GH_ParamAccess.item, false);

            Params.Input[1].Optional = true;
            Params.Input[1].AddVolatileDataList(new Grasshopper.Kernel.Data.GH_Path(0), null);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "Elements", "The elements with updated section properties", GH_ParamAccess.list);
            pManager.AddGenericParameter("Utilisations", "Utilisations", "The critical utilisation for the design elements", GH_ParamAccess.list);
            pManager.AddGenericParameter("Critical values", "CritVal", "The critical values of the utilisation", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Grasshopper_Engine.DataUtils.Run(DA, 8))
            {
                List<DesignElement> elems = Grasshopper_Engine.DataUtils.GetDesignElements(DA, 0);
                List<string> loadcases = Grasshopper_Engine.DataUtils.GetDataList<string>(DA, 1);
                IResultAdapter server = Grasshopper_Engine.DataUtils.GetGenericData<IResultAdapter>(DA, 2);
                string key = Grasshopper_Engine.DataUtils.GetData<string>(DA, 3);
                bool allowOffset = Grasshopper_Engine.DataUtils.GetData<bool>(DA, 4);
                List<SteelSection> secProps = Grasshopper_Engine.DataUtils.GetDataList<SteelSection>(DA, 5);
                double minUtil = Grasshopper_Engine.DataUtils.GetData<double>(DA, 6);
                double maxUtil = Grasshopper_Engine.DataUtils.GetData<double>(DA, 7);
                bool parallel = Grasshopper_Engine.DataUtils.GetData<bool>(DA, 9);

                SteelSectionSizer secSizer = new SteelSectionSizer(elems, loadcases, server, key, minUtil, maxUtil, secProps);
                secSizer.ParallelChecking = parallel;
                secSizer.Mode = (SizerMode)m_SelectedOption[0];


                m_critVals.Clear();
                m_desElems.Clear();
                m_utils.Clear();

                secSizer.Run(out m_desElems, out m_utils, out m_critVals);

                if (m_critVals.Max() > maxUtil)
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "At least one element was not able to find a suitable section and has a utilisation higher than the limit");


                for (int i = 0; i < m_desElems.Count; i++)
                {
                    m_tonnageDifference.Add(ElemWeight(m_desElems[i]) - ElemWeight(elems[i]));
                }

                List<double> tonnageBefore = elems.Select(x => x.SectionProperty.GrossArea * x.SectionProperty.Material.Density * x.Length).ToList();
                

            }

            DA.SetDataList(0, m_desElems);
            DA.SetDataList(1, m_utils);
            DA.SetDataList(2, m_critVals);
            DA.SetDataList(3, m_tonnageDifference);
        }

        private double ElemWeight(DesignElement elem)
        {
            return elem.Length * elem.SectionProperty.GrossArea * elem.SectionProperty.Material.Density;
        }
    }
}
