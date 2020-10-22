/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.Engine.Grasshopper;
using BH.UI.Grasshopper.Properties;
using BH.UI.Grasshopper.Templates;
using Grasshopper.Kernel;
using System;
using Rhino;
using Rhino.DocObjects;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;

namespace BH.UI.Grasshopper.Templates
{
    public abstract class BakeableParam<T> : BHoMParam<T>, IGH_BakeAwareObject where T : class, IGH_Goo
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override bool IsPreviewCapable
        {
            get
            {
                if (VolatileDataCount < m_MaxItemsPreview || m_ForcePreview && !Hidden)
                {
                    // Setting IsPreviewCapable from true to false clears the grasshopper geometry cache
                    // be mindful to use m_ForcePreview and Hidden variables only in case the number of objects
                    // to display is actually high
                    this.ClearRuntimeMessages();
                    return base.IsPreviewCapable;
                }
                Engine.Reflection.Compute.RecordNote("Preview has been disabled to prevent a slowdown due to the high number of objects." +
                    "Right click and set the a new items limit to force the preview at your own risk.");
                Helpers.ShowEvents(this, Engine.Reflection.Query.CurrentEvents());
                return false;
            }
        }

        public bool IsBakeCapable { get; } = true;


        /***************************************************/
        /**** IGH_BakeAwareObject methods               ****/
        /***************************************************/

        public void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            foreach (Engine.Grasshopper.Objects.GH_BakeableObject<T> item in this.VolatileData.AllData(true))
            {
                Guid guid;
                if (item.BakeGeometry(doc, null, out guid))
                    obj_ids.Add(guid);
            }
        }

        /*******************************************/

        public void BakeGeometry(RhinoDoc doc, ObjectAttributes att, List<Guid> obj_ids)
        {
            foreach (Engine.Grasshopper.Objects.GH_BakeableObject<T> item in this.VolatileData.AllData(true))
            {
                Guid guid;
                if (item.BakeGeometry(doc, att, out guid))
                    obj_ids.Add(guid);
            }
        }

        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BakeableParam(string name, string nickname, string description, string category, string subcategory)
            : base(name, nickname, description, category, subcategory)
        {
        }

        /*******************************************/
    }
}

