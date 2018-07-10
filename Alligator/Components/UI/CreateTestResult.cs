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
using BH.oM.Testing;
using Grasshopper.Kernel.Special;

namespace BH.UI.Alligator.Base
{
    public class CreateTestResult : GH_Component
    {

        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("3e7c03a2-831b-4b2c-a266-fa17155ab1a4");

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.CreateBHoM;

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateTestResult() : base("Create Test Result", "TestResult", "Creates a test result from a preformed test", "Alligator", " UI") { }





        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tested Component", "TestComp", "The component tested", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("TestResult", "Result", "The result outcome of the test", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Issue", "Issue", "Issue raised corresponding to the preformed test", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Test Result", "TestResult", "Generated Test Result", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            if (Params.Input[0].SourceCount == 0)
                return;

            //Get the sorce component from the first input
            MethodCallTemplate component = this.Params.Input[0].Sources[0].Attributes.GetTopLevel.DocObject as MethodCallTemplate;

            MethodBase method;

            //Get the method from the component
            if (component != null)
            {
                method = component.GetMethod();
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed to collect method from component");
                return;
            }


            List<bool> results = new List<bool>();

            if (!DA.GetDataList(1, results)) return;


            TestResult result = new TestResult()
            {
                Method = method,
                Results = results
            };



            BHoMObject issue = null;
            if (DA.GetData(2, ref issue))
            {
                result.Issue = issue as BH.oM.Planning.Issue;
            }

            DA.SetData(0, result);
        }
    }
}
