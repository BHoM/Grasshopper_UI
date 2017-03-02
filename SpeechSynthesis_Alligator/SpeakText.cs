using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace Alligator.SpeechSynthesis
{
    public class SpeakText : GH_Component
    {

        public SpeakText() : base("Speak", "Speak Text", "Speak text using speech synthesis engine", "Alligator", "Speech")
        {

        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{C46A35AC-F702-476D-8106-FC34BC134981}");
            }
        }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("text", "text", "text to speak", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
           
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            SpeechSynthesis_Engine.SpeakText m_SpeakText = new SpeechSynthesis_Engine.SpeakText();

            string text = null;
            

            DA.GetData<string>(0, ref text);

            m_SpeakText.Speak(text);

        }



    }
}
