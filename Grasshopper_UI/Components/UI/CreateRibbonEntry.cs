/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using BH.Engine.Serialiser;
using BH.oM.UI;
using BH.UI.Base;
using BH.UI.Grasshopper.Templates;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Grasshopper.Components.UI
{
    public class CreateRibbonEntry : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.CreateRibbonEntry; } }

        public override Guid ComponentGuid { get { return new Guid("6CA7A341-11C0-440F-8A5D-909B7E2DEB29"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateRibbonEntry() : base("CreateRibbonEntry", "CreateRibbonEntry", "Convert an input component into a BHoM CustomRibbonEntry", "BHoM", "UI") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("component", "component", "component to be turned into a BHoM CustomRibbonEntry", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddTextParameter("iconFile", "iconFile", "Image file to use for the icon of the component. We recommend using a png of size 24x24.", GH_ParamAccess.item);
            pManager.AddTextParameter("tabName", "tabName", "Name of the ribbon tab under which the component wil be found", GH_ParamAccess.item);
            pManager.AddTextParameter("category", "category", "Name of the category containing the component (each ribbon is sub-divided in categories).", GH_ParamAccess.item);
            pManager.AddIntegerParameter("groupIndex", "groupIndex", "Each category is divided in section. If you want your component to be added to a separate section, use an index higher index than 1.", GH_ParamAccess.item, 1);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ribbonEntry", "ribbonEntry", "Resulting BHoM ribbonEntry", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Base.Compute.ClearCurrentEvents();

            try
            {
                // Get the input component
                IGH_Param source = Params.Input[0].Sources.FirstOrDefault();
                if (source == null)
                    return;

                CallerComponent component = source.Attributes.GetTopLevel.DocObject as CallerComponent;
                if (component == null)
                    return;

                Caller caller = component.Caller;
                if (caller == null) 
                    return;

                object item = caller.SelectedItem;
                if (item == null) 
                    return;

                string iconFile = "";
                DA.GetData(1, ref iconFile);
                Bitmap icon = LoadIcon(iconFile);

                string tabName = "";
                DA.GetData(2, ref tabName);

                string category = "";
                DA.GetData(3, ref category);

                int groupIndex = 1;
                DA.GetData(4, ref groupIndex);

                CustomRibbonEntry entry = new CustomRibbonEntry
                {
                    CallerType = caller.GetType(),
                    ItemJson = item.ToJson(),
                    Icon = icon,
                    TabName = tabName,
                    Category = category,
                    GroupIndex = groupIndex
                };

                DA.SetData(0, entry);

                Helpers.ShowEvents(this, BH.Engine.Base.Query.CurrentEvents());
            }
            catch (Exception e)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
            }
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static Bitmap LoadIcon(string iconFile)
        {
            Bitmap icon = null;
            try
            {
                if (string.IsNullOrEmpty(iconFile))
                    BH.Engine.Base.Compute.RecordWarning("The file path for the icon was not provided, the default icon for this type of BHoM component will be used.");
                else if (!File.Exists(iconFile))
                    BH.Engine.Base.Compute.RecordError("The file provided for the icon doesn't exist");
                else
                    icon = new Bitmap(iconFile);
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError(e, "Failed to create icon for the entry");
            }

            return icon;
        }

        /*******************************************/
    }
}






