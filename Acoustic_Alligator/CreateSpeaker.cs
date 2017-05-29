using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;

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
            pManager.AddGenericParameter("Speaker direction", "V", "Main emissive direction of speaker", GH_ParamAccess.list);
            pManager.AddTextParameter("Speaker Category", "T", "Category of speaker for directivity specification", GH_ParamAccess.list);


            //indexing default values
            pManager[2].Optional = true;
            Param_String param2 = (Param_String)pManager[2];
            param2.PersistentData.Append(new GH_String("Omni"));
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
            if (!DA.GetDataList(1, dir)) { dir.Add(new BHG.Vector(1,0,0)); }
            if (!DA.GetDataList(2, cat)) { cat.Add("Omni"); }

            for (int i=0; i<pos.Count;i++)
            {
                BHA.Speaker speaker = new BHA.Speaker(pos[i], dir.Count==1?dir[0]:dir[i], cat.Count==1?cat[0]:cat[i]);
                speakers.Add(speaker);
            }
                        
            DA.SetDataList(0, speakers);
        }
    }
}
