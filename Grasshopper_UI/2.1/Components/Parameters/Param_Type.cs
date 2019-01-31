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

using BH.UI.Grasshopper.Properties;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Grasshopper.Objects
{
    public class Param_Type : GH_PersistentParam<Engine.Grasshopper.Objects.GH_Type>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.Type_Param;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("AA7DDCDC-2789-4A23-88AD-E1E4CD84FB37");

        public override string TypeName { get; } = "Type";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = false;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_Type()
            : base(new GH_InstanceDescription("Object Type", "Type", "Represents the type of an object", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool Read(GH_IReader reader)
        {
            Engine.Reflection.Compute.ClearCurrentEvents();
            bool success = base.Read(reader);
            Logging.ShowEvents(this, Engine.Reflection.Query.CurrentEvents());
            return success;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref Engine.Grasshopper.Objects.GH_Type value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<Engine.Grasshopper.Objects.GH_Type> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
