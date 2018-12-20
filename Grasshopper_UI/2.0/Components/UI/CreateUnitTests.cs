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
    public class CreateUnitTests : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("A8CBEBB8-2936-44C5-B104-BB87588A93A8");

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.CreateBHoM; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateUnitTests() : base("Create Unit Tests", "CreateTest", "Creates unit tests from teh components on the canvas", "BHoM", "Test") {}


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
                    source = BH.Engine.Rhinoceros.Convert.ToBHoM(source as dynamic);

                results.Add(source);
            }

            return results;
        }

        /*******************************************/
    }
}