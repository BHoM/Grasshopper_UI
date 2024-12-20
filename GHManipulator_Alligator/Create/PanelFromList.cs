/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BH.UI.Alligator.Base
{
    public class PanelFromList : GH_Component
    {
        // Initializes a new instance of the DuplicateComponent class.
        public PanelFromList() : base("PanelFromList", "PanelFromList", "Creates Groups with Panels inside them. Each group has a GroupName and template", "Alligator", "GHManipulator") { }

        // Assigns an id-number to the component.
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("884FE880-53DA-4844-824E-DA6E4F98BC73");
            }
        }

        // Registers all the input parameters for this component.
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("data list", "data", "list of strings used to create the panels", GH_ParamAccess.list);
            pManager.AddTextParameter("group name", "groupName", "name to give to the group containing the created panels", GH_ParamAccess.item);
            pManager.AddGenericParameter("template", "template", "panel used as template for replication and position", GH_ParamAccess.item);
            pManager.AddBooleanParameter("trigger", "trigger", "triggers the panel creation", GH_ParamAccess.item);
        }

        // Registers all the output parameters for this component. There are none!
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            
        }

        // This is the method that actually does the work.
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Component Component = this;
            GH_Document GrasshopperDocument = this.OnPingDocument();

            // Creating input parameters
            List<string> data = new List<string>();
            string groupName = "";
            bool trigger = false;
            object template = null;

            // Getting the data from Grasshopper
            DA.GetDataList<string>(0, data);
            DA.GetData<string>(1, ref groupName);
            DA.GetData<object>(2, ref template);
            DA.GetData<bool>(3, ref trigger);

            // If the botton is pressed it will proceed
            if (!trigger) return;

            // Detecting the the source parameter for the templateInput
            Grasshopper.Kernel.IGH_Param templateInput = Component.Params.Input[2];
            IList<Grasshopper.Kernel.IGH_Param> sources = templateInput.Sources;
            if (!sources.Any()) return;
            IGH_DocumentObject templateComp = sources[0].Attributes.GetTopLevel.DocObject;


            // Gets component attributes like the bounds of the Panel which is used to shift 
            //the next one and get the size of the panels
            IGH_Attributes att = templateComp.Attributes;
            RectangleF bounds = att.Bounds;
            int vShift = (int)Math.Round(bounds.Height) + 10;
            float refX = bounds.X;
            float refY = bounds.Y + 30 + vShift;

            // Creating a Grasshopper Group g and assignning a nickname and color to it. 
            //Adding group g to the GrasshopperDocument
            Grasshopper.Kernel.Special.GH_Group g = new Grasshopper.Kernel.Special.GH_Group();
            g.NickName = groupName;
            g.Colour = Grasshopper.GUI.Canvas.GH_Skin.group_back;
            GrasshopperDocument.AddObject(g, false);
            List<IGH_Component> components = new List<IGH_Component>();


            // For-loop used to create panels and assign properties to it (size, datalist...) 
            int nbCopy = data.Count;
            for (int i = 0; i < nbCopy; i++)
            {
                Grasshopper.Kernel.Special.GH_Panel panel = new Grasshopper.Kernel.Special.GH_Panel();
                panel.CreateAttributes();
                panel.SetUserText(data[i]);
                panel.Attributes.Bounds = bounds;
                panel.Attributes.Pivot = new PointF(refX, refY + i * vShift);
                GrasshopperDocument.AddObject(panel, false);

                g.AddObject(panel.InstanceGuid);
            }
            GrasshopperDocument.DeselectAll();

        }
        private IGH_Param getParam(IGH_DocumentObject o,int index,bool isInput)
        {
            IGH_Param result = null;

            if (o is IGH_Component)
            {
                IGH_Component p = o as IGH_Component;
                if (isInput)
                    result = p.Params.Input[index];
                else
                    result = p.Params.Output[index];
            }
            else
            {
                result = o as IGH_Param;
            }
            return result;   
            }
        }
    }






