using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.UI.Alligator.Templates;
using System.IO;
using System.Reflection;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using BH.UI.Alligator.Base.NonComponents.Others;
using BH.oM.Reflection.Testing;
using Grasshopper.Kernel.Special;

namespace BH.UI.Alligator.Base
{
    public class CreateUnitTests : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("A8CBEBB8-2936-44C5-B104-BB87588A93A8");

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.CreateBHoM; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateUnitTests() : base("Create Unit Tests", "CreateTest", "Creates unit tests from teh components on the canvas", "Alligator", " UI") {}


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Active", "Active", "Execute the component", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tests", "Tests", "units tests created", GH_ParamAccess.list);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool active = false; DA.GetData(0, ref active);

            if (active)
            {
                GH_Document doc = OnPingDocument();
                List<UnitTest> result = new List<UnitTest>();

                foreach (var component in CollectComponents(doc))
                    result.Add(new UnitTest { Method = component.GetMethod(), Data = CollectData(component) });

                DA.SetDataList(0, result);
            }
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private List<MethodCallTemplate> CollectComponents(GH_Document doc)
        {
            List<MethodCallTemplate> components = doc.Objects.OfType<MethodCallTemplate>().ToList();

            foreach (GH_Cluster cluster in doc.Objects.OfType<GH_Cluster>())
                components.AddRange(CollectComponents(cluster.Document("")));

            return components;
        }


        /*******************************************/

        private List<TestData> CollectData(MethodCallTemplate component)
        {
            BH_StructureIterator it = new BH_StructureIterator(component);
            try
            {
                if (this.Params.Input.Count == 0 || this.Params.OnlyTreeParameters)
                    return CollectData(component, it);
                else if (this.Params.OnlyTreeAndListParameters)
                    return CollectData_TreeAndListAccess(component, it);
                else
                    return CollectData_MixedAccess(component, it);
            }
            catch
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Failed to collect test for component " + component.ToString());
                return null;
            }
        }

        /*******************************************/

        private List<TestData> CollectData(MethodCallTemplate component, BH_StructureIterator it)
        {
            List<TestData> cases = new List<TestData>();
            cases.Add(component.GetAllCurrentData(it));

            return cases;
        }

        /*******************************************/

        private List<TestData> CollectData_TreeAndListAccess(MethodCallTemplate component, BH_StructureIterator it)
        {
            List<TestData> cases = new List<TestData>();

            while (!it.Document.AbortRequested)
            {
                cases.Add(component.GetAllCurrentData(it));
                if (it.AbortSolution)
                    return cases;

                if (it.IncrementBranchIndices())
                {
                    it.IncrementIteration();
                    continue;
                }
                return cases;
            }

            return cases;
        }

        /*******************************************/

        private List<TestData> CollectData_MixedAccess(MethodCallTemplate component, BH_StructureIterator it)
        {
            List<TestData> cases = new List<TestData>();
            while (!it.Document.AbortRequested)
            {
                cases.Add(component.GetAllCurrentData(it));
                if (it.AbortSolution)
                    return cases;

                if (it.IncrementItemIndices() || it.IncrementBranchIndices())
                {
                    it.IncrementIteration();
                    continue;
                }
                return cases;
            }

            return cases;
        }


        /*******************************************/

        private List<object> GetData(IGH_Param p)
        {
            List<object> results = new List<object>();

            foreach (IGH_Goo goo in p.VolatileData.AllData(false))
            {
                object source = goo;
                while (source is IGH_Goo)
                    source = ((IGH_Goo)source).ScriptVariable();

                if (source.GetType().Namespace.StartsWith("Rhino.Geometry"))
                    source = Engine.Rhinoceros.Convert.ToBHoM(source as dynamic);

                results.Add(source);
            }

            return results;
        }

        /*******************************************/
    }
}