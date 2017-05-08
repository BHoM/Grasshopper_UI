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
        public CreateSpeaker() : base("Create Speaker", "Spk", "Create BHoM Acoustic Speaker", "Alligator", "Acoustics") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("8B40B2DF-8245-4230-8BCE-006427426F11");
            }
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Acoustic_Alligator.Properties.Resources.BHoM_Acoustics_Speaker; }
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Position", "P", "Position of source", GH_ParamAccess.list);
            pManager.AddGenericParameter("Speaker direction", "Direction", "Main emissive direction of speaker", GH_ParamAccess.list);
            pManager.AddGenericParameter("Speaker Category", "Category", "Category of speaker for directivity specification", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM Speaker", "Speaker", "BHoM Speaker Position and Direction", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHA.Speaker> speakers = new List<BHA.Speaker>();

            List<BHG.Point> pos = new List<BHG.Point>();            //Add automatic conversion if Rhino.Point3d
            List<BHG.Vector> dir = new List<BHG.Vector>();          //Add automatic conversion if Rhino.Vector3d         
            List<String> cat = new List<String>();

            if (!DA.GetDataList(0, pos)) { return; }
            if (!DA.GetDataList(1, dir)) { return; }
            if (!DA.GetDataList(2, cat)) { return; }                // if they are diffferent lenght fill the variable with default values

            for (int i=0; i<pos.Count;i++)
            {
                BHA.Speaker speaker = new BHA.Speaker(pos[i], dir[i] = null, cat[i] = "Omni");
                speakers.Add(speaker);
            }
                        
            DA.SetDataList(0, speakers);
        }
    }
}
