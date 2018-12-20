/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.UI.Grasshopper.Templates;
using System.IO;
using System.Reflection;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using BH.UI.Grasshopper.Base.NonComponents.Others;
using BH.oM.Testing;
using Grasshopper.Kernel.Special;

namespace BH.UI.Grasshopper.Base
{
    public class CreateTestResult : GH_Component
    {

        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("3e7c03a2-831b-4b2c-a266-fa17155ab1a4");

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.CreateBHoM;

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateTestResult() : base("Create Test Result", "TestResult", "Creates a test result from a preformed test", "BHoM", "Test") { }





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
