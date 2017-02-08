using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using System.Threading;
using GHE = Grasshopper_Engine;


namespace Alligator.SpeechRecognition
{
    public class DetectCommand : GH_Component
    {

        private SpeechRecognition_Engine.CommandProcessor m_CommandProcessor;
        private string m_command;
        bool CommandDetected = false;


        public DetectCommand() : base("DetectSpeech", "Process Voice Command", "Process voice commands based on finite library of grammar", "Alligator", "Speech")
        {
            m_CommandProcessor = new SpeechRecognition_Engine.CommandProcessor();
            m_CommandProcessor.CommandProcessed += CommandProcessed;        
        }

        private void CommandProcessed(string data)
        {
            m_command = data;
            CommandDetected = true;
            ExpireSolution(true);
        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{4448851A-683B-4F44-9A38-04B77640B465}");
            }
        }

        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("grammar", "grammar", "List of strings defining grammar to detect command from", GH_ParamAccess.list);
            pManager.AddBooleanParameter("active", "active", "waiting to recieve voice command", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("command", "command", "command detected", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> commands = new List<string>();
            bool active = false;

            DA.GetDataList<string>(0, commands);
            DA.GetData<bool>(1, ref active);

            if (!active)
            {
                m_CommandProcessor.Disable();
                return;
            }

            if(!CommandDetected)
            {
                m_CommandProcessor.Initialise(commands.ToArray());
                m_CommandProcessor.Enable();
            }
               

            DA.SetData(0, m_command);

            CommandDetected = false;

        }



    }
}
