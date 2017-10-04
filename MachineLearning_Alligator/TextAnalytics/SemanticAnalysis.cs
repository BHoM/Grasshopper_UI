using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BH.oM.Geometry;
using BH.oM.Base;
using BH.UI.Alligator.Base;
using System.Linq;
using BH.Engine.MachineLearning;
using Grasshopper.Kernel.Parameters;
using System.Windows.Forms;

namespace BH.UI.Alligator.MachineLearning
{
    public class SemanticAnalysis : GH_Component, IGH_VariableParameterComponent
    {
        public SemanticAnalysis() : base("SemanticAnalysis", "Semantics", "Semantic analysis of a text file based on the Stanford CoreNPL neural network", "Alligator", "MachineLearning") { }
        protected override System.Drawing.Bitmap Icon { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("95a29710-43f4-4681-bb7b-2042b2d16469"); } }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Grasshopper.Kernel.Parameters.Param_GenericObject();
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance() { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "Path", "Path of the text file to analyse", GH_ParamAccess.item);
            pManager.AddTextParameter("Text", "Text", "Text to analyse. If text is provided, <Path> will be ignored", GH_ParamAccess.item);
            pManager.AddTextParameter("Property", "Property", "Property to extract from analysis. To have a full list of properties, please check CoreNLP Documentation", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output", "Output", "Semantic analysis annotation complete output", GH_ParamAccess.item);
            pManager.AddTextParameter("Sentences", "Sentences", "Sentences split result of the annotation", GH_ParamAccess.list);
        }

        protected override void BeforeSolveInstance()
        {
            StaticParamCount = Params.Output.Count;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "", text = "", analysisText = "";
            string annotation = "";

            List<string> properties = new List<string>();
            DA.GetDataList(2, properties);

            if ((!DA.GetData(0, ref path)) && (DA.GetData(1, ref text)))
            {
                annotation = BH.Engine.MachineLearning.Analyse.Semantics(text);
            }
            else if ((DA.GetData(0, ref path)) && (!DA.GetData(1, ref text)))
            {
                analysisText = System.IO.File.ReadAllText(path);
                annotation = BH.Engine.MachineLearning.Analyse.Semantics(analysisText);
            }
            else { return; }

            for (int i = 0; i < properties.Count; i++)
            {
                List<string> values = new List<string>();
                values.AddRange(BH.Engine.MachineLearning.Analyse.ParseAnnotation(annotation, properties[i]).Select(x => x.ToString()));
                if (m_Outputs.ContainsKey(properties[i]))
                {
                    continue;
                }
                else
                {
                    m_Outputs.Add(properties[i], values);
                }
            }

            List<string> sentences = (annotation.getSentences());

            DA.SetData(0, annotation);
            DA.SetDataList(1, sentences);
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Update Outputs", Menu_DoClick);
        }
        private void Menu_DoClick(object sender, EventArgs e)
        {
            UpdateOutputs();
        }

        protected override void AfterSolveInstance()
        {
            UpdateOutputs();
        }
        private void UpdateOutputs()
        {
            List<string> keys = m_Outputs.Keys.ToList();

            int nbNew = keys.Count();
            int nbOld = Params.Output.Count() - StaticParamCount;

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
            {
                Params.Output[i + StaticParamCount].NickName = keys[i];
            }

            for (int i = nbOld; i > nbNew; i--)
                Params.UnregisterOutputParameter(Params.Output[i + StaticParamCount]);

            for (int i = nbOld; i < nbNew; i++)
            {
                Grasshopper.Kernel.Parameters.Param_GenericObject newParam = new Grasshopper.Kernel.Parameters.Param_GenericObject();
                newParam.NickName = keys[i];
                Params.RegisterOutputParam(newParam);
            }
            this.OnAttributesChanged();
            if (nbNew != nbOld)
            {
                ExpireSolution(true);
            }
        }
        private Dictionary<string, List<string>> m_Outputs = new Dictionary<string, List<string>>();
        private int StaticParamCount = 0;
    }
}