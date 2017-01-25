using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using BHG = BHoM.Geometry;
using BHA = BHoM.Acoustic;

namespace Acoustic_Alligator
{
    public class CreateSpeaker : GH_Component
    {
        public CreateSpeaker() : base("Create Speaker", "Create Speaker", "Create BHoM Speaker", "Alligator", "Acoustics") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("8B40B2DF-8245-4230-8BCE-006427426F11");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Speaker Line", "Line", "Speaker Position and Direction", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM Speaker", "Speaker", "BHoM Speaker Position and Direction", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHA.Speaker> speakers = new List<BHA.Speaker>();

            List<Rhino.Geometry.Line> lines = new List<Rhino.Geometry.Line>();
            List<Rhino.Geometry.Point3d> pos = new List<Rhino.Geometry.Point3d>();
            List<Rhino.Geometry.Vector3d> dir = new List<Rhino.Geometry.Vector3d>();

            if (!DA.GetDataList(0, lines)) { return; }

            foreach (Rhino.Geometry.Line line in lines)
            {
                pos.Add(line.PointAt(0));
                dir.Add(line.Direction);
            }

            for (int i = 0; i < pos.Count; i++)
            {
                BHA.Speaker speaker = new BHA.Speaker(new BHG.Point(pos[i].X, pos[i].Y, pos[i].Z), new BHG.Vector(dir[i].X, dir[i].Y, dir[i].Z), "Speaker");
                speakers.Add(speaker);
            }

            DA.SetDataList(0, speakers);

        }
    }
}
