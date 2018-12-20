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

using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BH.UI.Alligator.Base
{
    public class LinksBetweenGroups : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public LinksBetweenGroups()
          : base("Links Between Groups of Components", "LinkGroups",
              "Create links between two groups of components",
              "Alligator", "GHManipulator")
        { }


        public override Guid ComponentGuid
        {
            get { return new Guid("46B57C01-E253-4EC1-8DAF-1B2CF55E1C48"); }
        }
        //-------------------------------------------

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
            pManager.AddTextParameter("source group", "sourceGroup", "group of components that will be connected from", GH_ParamAccess.item);
            pManager.AddTextParameter("target groups", "targetGroup", "group of components that will be connected into", GH_ParamAccess.item);
            pManager.AddIntegerParameter("sourceindex", "sourceIndex", "index of output connector for source components", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("targetindex", "targetIndex", "index of input connector for target components", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("trigger", "trigger", "triggers the link creation", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Component Component = this;
            GH_Document GrasshopperDocument = this.OnPingDocument();

            // Create input parameters
            bool trigger = false;
            string sourceGroup = null;
            string targetGroup = null;
            int sourceIndex = 0;
            int targetIndex = 0;

            // Assign input data
            DA.GetData (0, ref sourceGroup);
            DA.GetData(1, ref targetGroup);
            DA.GetData(2,  ref sourceIndex);
            DA.GetData(3, ref targetIndex);
            DA.GetData(4, ref trigger);


            // --------------------------------------------------------------------------------------------------------------------------
            if (!trigger) return;

            List<IGH_Param> targetParam = new List<IGH_Param>();
            List<IGH_Param> sourceParam = new List<IGH_Param>();

            foreach (IGH_DocumentObject docObject in GrasshopperDocument.Objects) //for every GH component in document
            {

                // if the pathname of the actual object is the same as the Pathname of the targetgroup
                // put the docObject in the group "gp" as a GH special group
                // then take out the Params of the target objects
                if (docObject.Attributes.PathName == "Group (" + targetGroup + ")")
                {
                    Grasshopper.Kernel.Special.GH_Group gp = docObject as Grasshopper.Kernel.Special.GH_Group;
                    targetParam = getParams(gp.Objects(), targetIndex, sourceIndex, true);
                }
                // the same as above but for the source objects
                else if (docObject.Attributes.PathName == "Group (" + sourceGroup + ")")
                {

                    Grasshopper.Kernel.Special.GH_Group gp = docObject as Grasshopper.Kernel.Special.GH_Group;
                    sourceParam = getParams(gp.Objects(), sourceIndex, sourceIndex, false);

                }

            }

            int nbI = targetParam.Count();
            int nbO = sourceParam.Count();
            int nb = Math.Max(nbI, nbO);
            for (int i = 0; i < nb; i++)
            {
                targetParam[i % nbI].AddSource(sourceParam[i % nbO]);
            }


        }

        // The function that takes out the params from the source and target objects
        // it finds out which components are sources and targets and which node to connect them to.

        private List<IGH_Param> getParams(List<IGH_DocumentObject> g, int index, int counter, bool isInput)
        {
            List<IGH_Param> pList = new List<IGH_Param>();

            for (int i = -1; i < index; i++) /*for loop*/
            {
                foreach (IGH_DocumentObject o in g)
                {
                    if (o is IGH_Component)
                    {
                        IGH_Component p = o as IGH_Component;
                        if (isInput)
                            pList.Add(p.Params.Input[index]); /*i instead for index*/
                        else
                          if (i != index - 1)
                        {

                            pList.Add(p.Params.Output[i + 1 + counter]);
                        }

                    }
                    else
                    {
                        IGH_Param p = o as IGH_Param;
                        pList.Add(p);
                    }
                }
            }

            return pList;
        }
    }
}
     
    
    

