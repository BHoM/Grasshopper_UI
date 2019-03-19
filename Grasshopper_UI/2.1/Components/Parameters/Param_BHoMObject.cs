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
using BH.UI.Grasshopper.Templates;
using BH.UI.Templates;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BH.UI.Grasshopper.Objects
{
    public class Param_BHoMObject : GH_PersistentParam<Engine.Grasshopper.Objects.GH_BHoMObject>, IGH_PreviewObject
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.BHoMObject_Param;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("1DEF3710-FD5B-4617-BCF6-B6293C5C6530");

        public override string TypeName { get; } = "BHoM Object";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = true;

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Preview_ComputeClippingBox(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_BHoMObject()
            : base(new GH_InstanceDescription("BHoM object", "BHoM", "Represents a collection of generic BHoM objects", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            Preview_DrawMeshes(args);
        }

        /*******************************************/

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            Preview_DrawWires(args);
        }

        /*******************************************/

        public override bool Read(GH_IReader reader)
        {
            Engine.Reflection.Compute.ClearCurrentEvents();
            bool success = base.Read(reader);
            Logging.ShowEvents(this, Engine.Reflection.Query.CurrentEvents());
            return success;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref Engine.Grasshopper.Objects.GH_BHoMObject value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<Engine.Grasshopper.Objects.GH_BHoMObject> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override string HtmlHelp_Source()
        {
            if (this.Attributes.IsTopLevel)
            {
                Engine.Grasshopper.Objects.GH_BHoMObject obj = this.m_data.get_FirstItem(true);
                if (obj != null && obj.Value != null)
                {
                    BH.Engine.Reflection.Compute.OpenHelpPage(obj.Value.GetType());
                }
            }
            else if (typeof(CallerComponent).IsAssignableFrom(this.Attributes.GetTopLevel.DocObject.GetType()))
            {
                CallerComponent parent = this.Attributes.GetTopLevel.DocObject as CallerComponent;
                if (parent.Caller != null && parent.Caller.SelectedItem is MethodInfo)
                {
                    MethodInfo method = parent.Caller.SelectedItem as MethodInfo;
                    Type type;
                    if (parent.Params.IsInputParam(this))
                    {
                        ParameterInfo[] inputs = method.GetParameters();
                        int k = parent.Params.Input.FindIndex(p => p.NickName == this.NickName);
                        if (k < inputs.Length)
                            type = inputs[k].ParameterType;
                    }
                    else if (parent.Params.IsOutputParam(this))
                    {
                        type = method.ReflectedType;
                    }

                    BH.Engine.Reflection.Compute.OpenHelpPage(method.ReturnType);
                }
            }
            return base.HtmlHelp_Source();
        }
    }

    /*******************************************/
}
