using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHG = BH.oM.Geometry;
using BH.oM.Acoustic;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.Acoustic
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
            pManager.AddParameter(new BHoMGeometryParameter(), "Position", "P", "Position of source", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMGeometryParameter(), "Speaker direction", "V", "Main emissive direction of speaker", GH_ParamAccess.item);
            pManager.AddTextParameter("Speaker Category", "T", "Category of speaker for directivity specification", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM Speaker", "Speaker", "BHoM Speaker Position and Direction", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHG.Point pos = new BHG.Point();
            BHG.Vector dir = new BHG.Vector();
            string cat = "Omni";

            pos = DA.BH_GetData(0, pos);
            dir = DA.BH_GetData(1, dir);
            cat = DA.BH_GetData(2, cat);

            Speaker speaker = new Speaker(pos, dir, cat);
            DA.SetData(0, speaker);
        }
    }
}
