/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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

using BH.Engine.Reflection;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using GH = Grasshopper;

namespace BH.UI.Grasshopper.Templates
{
    public class BHoMParam<T> : GH_PersistentParam<T>, IGH_PreviewObject where T : class, IGH_Goo
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; }

        public override Guid ComponentGuid { get; }

        public override string TypeName { get; }

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = true;

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Preview_ComputeClippingBox(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMParam(string name, string nickname, string description, string category, string subcategory)
            : base(name, nickname, description, category, subcategory)
        {
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public virtual void OnSourceCodeClick(object sender = null, object e = null)
        {
            if (this.Attributes.IsTopLevel)
            {
                T obj = this.m_data.get_FirstItem(true);

                if (obj != null)
                {
                    //Using reflection as GH_Goo requires generic type constraints that can not be set on the level of the class
                    object value = obj.PropertyValue("Value");
                    if (value != null)
                    {
                        BH.Engine.Reflection.Compute.OpenHelpPage(value.GetType());
                    }
                }
            }
            else if (typeof(CallerComponent).IsAssignableFrom(this.Attributes.GetTopLevel.DocObject.GetType()))
            {
                CallerComponent parent = this.Attributes.GetTopLevel.DocObject as CallerComponent;
                if (parent.Caller != null && parent.Caller.SelectedItem is MethodInfo)
                {
                    Type type = null;
                    if (parent.Params.IsInputParam(this))
                    {
                        type = parent.Caller.InputParams.Find(p => p.Name == this.NickName).DataType;
                    }
                    else if (parent.Params.IsOutputParam(this))
                    {
                        type = parent.Caller.OutputParams.FirstOrDefault()?.DataType;
                    }
                    BH.Engine.Reflection.Compute.OpenHelpPage(type);
                }
            }
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

        protected override GH_GetterResult Prompt_Singular(ref T value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<T> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Source code", OnSourceCodeClick, Properties.Resources.BHoM_Logo);
        }

        /*******************************************/
    }

    /*******************************************/
}
