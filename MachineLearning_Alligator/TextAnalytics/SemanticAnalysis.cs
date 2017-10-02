using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace BH.UI.Alligator.MachineLearning
{
    public class SemanticAnalysis : GH_Component, IGH_VariableParameterComponent
    {
        /// <summary>
        /// Initializes a new instance of the SemanticAnalysis class.
        /// </summary>
        public SemanticAnalysis()
          : base("SemanticAnalysis", "Semantics",
              "Semantic analysis of a text file based on the Stanford CoreNPL neural network",
              "Alligator", "MachineLearning")
        {
        }

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

        public void VariableParameterMaintenance()
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "Path", "Path of the text file to analyse", GH_ParamAccess.item);
            pManager.AddTextParameter("Text", "Text", "Text to analyse. If text is provided, <Path> will be ignored", GH_ParamAccess.item);
            pManager.AddTextParameter("Property", "Property", "Property to extract from analysis. To have a full list of properties, please check CoreNLP Documentation", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output", "Output", "Semantic analysis annotation complete output", GH_ParamAccess.item);
            pManager.AddTextParameter("Sentences", "Sentences", "Sentences split result of the annotation", GH_ParamAccess.list);
            pManager.AddTextParameter("Fields", "Fields", "Fields", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "", text = "", analysisText = "";
            string annotation = "";

            List<string> properties = new List<string>();
            DA.GetDataList<string>(2, properties);


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

            Dictionary<string, List<object>> fields = new Dictionary<string, List<object>>();
            for (int i = 0; i < properties.Count; i++)
            {
                fields.Add(properties[i], BH.Engine.MachineLearning.SemanticAnalysis.ParseAnnotation(annotation, properties[i]));
            }

            List<string> sentences = (BH.Engine.MachineLearning.SemanticAnalysis.getSentences(annotation));

            DA.SetData(0, annotation);
            DA.SetDataList(1, sentences);
            DA.SetData(2, fields);
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
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("95a29710-43f4-4681-bb7b-2042b2d16469"); }
        }
    }
}