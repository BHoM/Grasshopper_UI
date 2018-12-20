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
using System.Linq;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Rhinoceros;
using System.Collections;
using Grasshopper.Kernel.Types;

namespace BH.UI.Grasshopper.Base
{
    public class GetProperty : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.BHoM_GetProperty; 

        public override Guid ComponentGuid { get; } = new Guid("E14EF77D-4F09-4CFB-AB75-F9B723212D00"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.hidden;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GetProperty() : base("Get Property", "GetProperty", "Get property of a BHoM object from the property name", "Grasshopper", " Engine") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("object", "object", "Object to get property from", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Property value", "value", "Value of the property", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            object obj = null;
            string key = "";

            DA.GetData(0, ref obj);
            DA.GetData(1, ref key);

            while (obj is IGH_Goo)
                obj = ((IGH_Goo)obj).ScriptVariable();

            try
            {
                object result = BH.Engine.Reflection.Query.PropertyValue(obj, key);

                if (result is IEnumerable && !(result is string) && !(result is IDictionary))
                {
                    if (typeof(IGeometry).IsAssignableFrom(((IEnumerable)result).GetType().GenericTypeArguments.First()))
                        DA.SetDataList(0, ((IEnumerable)result).Cast<IGeometry>().Select(x => Query.IsRhinoEquivalent(x.GetType()) ? x.IToRhino() : x));
                    else
                        DA.SetDataList(0, result as IEnumerable);
                }
                else
                {
                    if (Query.IsRhinoEquivalent(result.GetType()))
                        DA.SetData(0, ((IGeometry)result).IToRhino());
                    else
                        DA.SetData(0, result);
                }
            }
            catch(Exception e)
            {
                BH.Engine.Reflection.Compute.RecordError(e.ToString());
            }

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*******************************************/
    }
}
