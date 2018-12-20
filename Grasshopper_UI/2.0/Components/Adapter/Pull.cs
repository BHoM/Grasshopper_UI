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
using BH.UI.Grasshopper.Base;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using BH.Adapter;
using System.Linq;
using BH.oM.Geometry;
using BH.Engine.Rhinoceros;

namespace BH.UI.Grasshopper.Adapter
{
    public class Pull : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Pull; 

        public override Guid ComponentGuid { get; } = new Guid("BA3D716D-3044-4795-AC81-0FECC80781E3"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.hidden;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Pull() : base("Pull", "Pull", "Pull objects from the external software", "Grasshopper", " Adapter") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddGenericParameter("Query", "Query", "BHoM Query", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the pull", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "Objects", "Objects obtained from the query", GH_ParamAccess.list);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            BHoMAdapter adapter = null; DA.GetData(0, ref adapter);
            IQuery query = null; DA.GetData(1, ref query);
            CustomObject config = new CustomObject(); DA.GetData(2, ref config);
            bool active = false; DA.GetData(3, ref active);

            if (!active) return;

            if (query == null)
                query = new FilterQuery();

            Guid id = adapter.BHoM_Guid;
            if (id != m_AdapterId)
            {
                m_AdapterId = id;
                adapter.DataUpdated += Adapter_DataUpdated;
            }

            IEnumerable<object> objects = adapter.Pull(query, config.CustomData);
            objects = objects.Select(x => Query.IsRhinoEquivalent(x.GetType()) ? ((IGeometry)x).IToRhino() : x);
            DA.SetDataList(0, objects);

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*******************************************/

        private void Adapter_DataUpdated(object sender, EventArgs e)
        {
            ExpireSolution(true);
        }

        /*******************************************/

        private Guid m_AdapterId;
    }
}
