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
using BH.UI.Grasshopper.Base;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.Adapter;

namespace BH.UI.Grasshopper.Adapter
{
    public class UpdateProperty : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.UpdateProperty; 

        public override Guid ComponentGuid { get; } = new Guid("E050834D-F825-4299-BEA9-A3E067691925"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.hidden;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public UpdateProperty() : base("UpdateProperty", "UpdateProperty", "Update a property of objects from the external software", "Grasshopper", " Adapter") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddGenericParameter("Filter", "Filter", "Filer Query", GH_ParamAccess.item);
            pManager.AddTextParameter("Property", "Property", "Name of the property to change", GH_ParamAccess.item);
            pManager.AddGenericParameter("NewValue", "NewValue", "New value to assign to the property", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the pull", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[4].Optional = true;
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("#updated", "#updated", "Number of objects updated", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            BHoMAdapter adapter = null; DA.GetData(0, ref adapter);
            FilterRequest query = null; DA.GetData(1, ref query);
            string property = ""; DA.GetData(2, ref property);
            object value = null; DA.GetData(3, ref value);
            CustomObject config = new CustomObject(); DA.GetData(4, ref config);
            bool active = false; DA.GetData(5, ref active);

            if (!active) return;

            if (query == null)
                query = new FilterRequest();

            int nb = adapter.UpdateProperty(query, property, value, config.CustomData);
            DA.SetData(0, nb);

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*******************************************/
    }
}
