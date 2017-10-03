using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BH.oM.Geometry;
using BH.oM.Base;
using BH.UI.Alligator.Base;
using System.Linq;

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

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "", text = "", analysisText = "";
            string annotation = "";

            List<string> properties = new List<string>();
            DA.GetDataList(2, properties);

            if ((!DA.GetData(0, ref path)) && (DA.GetData(1, ref text)))
            {
                annotation = BH.Engine.MachineLearning.SemanticAnalysis.RunAnalysis(text);
            }
            else if ((DA.GetData(0, ref path)) && (!DA.GetData(1, ref text)))
            {
                analysisText = System.IO.File.ReadAllText(path);
                annotation = BH.Engine.MachineLearning.SemanticAnalysis.RunAnalysis(analysisText);
            }
            else { return; }

            for (int i = 0; i < properties.Count; i++)
            {
                List<string> values = new List<string>();
                values.AddRange(BH.Engine.MachineLearning.SemanticAnalysis.ParseAnnotation(annotation, properties[i]).Select(x=>x.ToString()));
                if (m_Outputs.ContainsKey(properties[i]))
                {
                    continue;
                }
                else
                {
                    m_Outputs.Add(properties[i], values);
                }
            }

            List<string> sentences = (BH.Engine.MachineLearning.SemanticAnalysis.getSentences(annotation));

            DA.SetData(0, annotation);
            DA.SetDataList(1, sentences);
        }

        protected override void AfterSolveInstance()
        {
            UpdateOutputs();
        }
        private void UpdateOutputs()
        {
            List<string> keys = m_Outputs.Keys.ToList();

            int nbNew = keys.Count();
            int nbOld = Params.Output.Count();

            for (int i = 2; i < Math.Min(nbNew, nbOld); i++)
            {
                Params.Output[i].NickName = keys[i];
            }

            for (int i = nbOld - 1 +2; i > nbNew - 1 +2 ; i--)
                Params.UnregisterOutputParameter(Params.Output[i]);

            for (int i = nbOld+2; i < nbNew+2; i++)
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
        /*
        protected override void AfterSolveInstance()
        {
            //base.AfterSolveInstance();
            Grasshopper.Kernel.Data.IGH_Structure keys = Params.Input[2].VolatileData;
            IEnumerable<string> properties = keys.AllData(true).Cast<string>();

            Grasshopper.Kernel.Data.IGH_Structure json = Params.Output[0].VolatileData;
            IEnumerable<string> annotation = keys.AllData(true).Cast<string>();

            for (int i = 0; i < keys.DataCount; i++)
            {
                
                if (Params.Output.Count > keys.DataCount + 1)
                {
                    if (Params.Output[i + 1].Name == keys.AllData(false).ToList()[i]) { Params.UnregisterOutputParameter(Params.Output[i + 1]); }
                }
                
                Param_GenericObject newParam = new Param_GenericObject();
                newParam.Name = properties.ElementAt(i).ToString();
                newParam.NickName = properties.ElementAt(i).ToString();
                newParam.Description = properties.ElementAt(i).ToString() + " field of analysed text";
                newParam.Access = GH_ParamAccess.list;
                Params.RegisterOutputParam(newParam);
                Params.Output[i+1].AddVolatileDataList(new Grasshopper.Kernel.Data.GH_Path(0), SemanticAnalysis_Engine.SemanticAnalysis.ParseAnnotation(annotation.ElementAt(0), properties.ElementAt(i)));
            }
        }
        */
        private Dictionary<string, List<string>> m_Outputs = new Dictionary<string, List<string>>();
    }
}