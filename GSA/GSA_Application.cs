using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSAToolkit;
using Grasshopper.Kernel;

namespace Alligator.GSA
{
    //public class GSAApp : GH_Component
    //{
    //    public GSAApp() : base("GSA Application", "GSAApp", "Creates a GSA Application", "GSA", "Application") { }

    //    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    //    {
    //        pManager.AddTextParameter("Filename", "F", "GSA Filename", GH_ParamAccess.item);
    //        pManager.AddGenericParameter("Settings", "S", "Application Settings", GH_ParamAccess.item);
    //        pManager[0].Optional = true;
    //        pManager[1].Optional = true;
    //    }

    //    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    //    {
    //        pManager.AddGenericParameter("Application", "A", "GSA Application", GH_ParamAccess.item);
    //    }

    //    protected override void SolveInstance(IGH_DataAccess DA)
    //    {
    //        RobotAdapter app = new RobotAdapter();
    //        DA.SetData(0, app);
    //    }

    //    public override Guid ComponentGuid
    //    {
    //        get { return new Guid("97b9ded4-41a2-4ded-b835-432332fb760a"); }
    //    }

    //    /// <summary> Icon (24x24 pixels)</summary>
    //    protected override System.Drawing.Bitmap Internal_Icon_24x24
    //    {
    //        get { return Robot.Properties.Resources.robot_app; }
    //    }

    //}
}
