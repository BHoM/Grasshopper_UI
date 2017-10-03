using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper_Engine;
using BHoM.Structural.Elements;
using BHoM.Structural.Loads;
using System.Xml;
using System.IO;
using System.Threading;

namespace Miscellaneous_Alligator
{
    public class ExportToFBeam : GH_Component
    {
        private SpinLock m_Lock = new SpinLock();
        bool m_IsLocked = false;
        public ExportToFBeam() : base("Export To FBeam", "ToFBeam", "Creates and FBeam Xml file from the input data", "Stucture", "Miscellaneous") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{AE851740-4945-404D-82B8-C66C5E0AE16D}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Filename", "F", "Filename of output Fbeam file", GH_ParamAccess.item);
            pManager.AddTextParameter("Group", "G", "Group of members", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bars", "B", "Bars to export", GH_ParamAccess.item);
            pManager.AddNumberParameter("Beam Spacing", "Sp", "Spacing between bars", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Support", "Su", "Span: 1 - Simply Supported, 2 - Fixed/Pinned, 3 - Fixed ends, 4 - Cantilever", GH_ParamAccess.item);
            pManager.AddGenericParameter("Loadcases", "LC", "Loadcases to export", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "L", "Loads to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "R", "Export data to xml", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            //
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (DataUtils.Run(DA,7))
            {
                string filename = DataUtils.GetData<string>(DA, 0);
                string group = DataUtils.GetData<string>(DA, 1);
                if (!filename.EndsWith(".xml")) filename += ".xml";
                Bar bar = DataUtils.GetGenericData<Bar>(DA, 2);
                double spacing = DataUtils.GetData<double>(DA, 3);
                int support = DataUtils.GetData<int>(DA, 4);
                string supportType = "";
                switch (support)
                {
                    case 1:
                        supportType = "SimplySupportedRollerLeft";
                        break;
                    case 2:
                        supportType = "FixedPinned";
                        break;
                    case 3:
                        supportType = "FixedEnds";
                        break;
                    case 4:
                        supportType = "CantileverLeft";
                        break;
                }
                
                List<ICase> cases = DataUtils.GetGenericDataList<ICase>(DA, 5);
                List<ILoad> loads = DataUtils.GetGenericDataList<ILoad>(DA, 6);

                XmlDocument doc = null;

                try
                {
                    m_Lock.Enter(ref m_IsLocked);

                    if (!File.Exists(filename))
                    {
                        //File.Create(filename);               
                        doc = FBeamFileGenerator.XmlGenerator.CreateFBeamDoc();
                    }
                    else
                    {
                        doc = new XmlDocument();
                        doc.Load(filename);
                    }
                    FBeamFileGenerator.XmlGenerator.AppendBeam(doc, bar, spacing, supportType, group, cases, loads);
                    doc.Save(filename);
                }
                finally
                {
                    if (m_IsLocked) m_Lock.Exit();
                    m_IsLocked = false;
                }
            }
        }
    }
}
