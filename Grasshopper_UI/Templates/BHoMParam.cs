/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using Grasshopper.GUI;
using BH.Engine.Serialiser;

namespace BH.UI.Grasshopper.Templates
{
    public class BHoMParam<T> : GH_PersistentParam<T>, IBHoMParam, IGH_PreviewObject where T : class, IGH_Goo
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; }

        public override Guid ComponentGuid { get; }

        public override string TypeName { get; }

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public bool Hidden { get; set; } = false;

        public virtual bool IsPreviewCapable { get; } = true;

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Preview_ComputeClippingBox(); } }

        public Type ObjectType { get; set; } = typeof(object);


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

        public virtual void OnMaxItemsPreviewChange(GH_MenuTextBox sender, string text)
        {
            int parsed;
            if (text != null && int.TryParse(text, out parsed))
                m_MaxItemsPreview = parsed != -1 ? parsed : int.MaxValue;
        }

        /*******************************************/

        public virtual void OnForcePreviewClick(object sender, EventArgs e)
        {
            m_ForcePreview = !m_ForcePreview;
        }

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
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("m_MaxItemsPreview", m_MaxItemsPreview);
            writer.SetBoolean("m_ForcePreview", m_ForcePreview);

            if (ObjectType != null)
                writer.SetString("ObjectType", ObjectType.ToJson());

            return base.Write(writer);
        }

        /*******************************************/

        public override bool Read(GH_IReader reader)
        {
            Engine.Base.Compute.ClearCurrentEvents();
            bool success = base.Read(reader);
            reader.TryGetInt32("m_MaxItemsPreview", ref m_MaxItemsPreview);
            reader.TryGetBoolean("m_ForcePreview", ref m_ForcePreview);

            string objectType = "";
            if (reader.TryGetString("ObjectType", ref objectType))
                ObjectType = Engine.Serialiser.Convert.FromJson(objectType) as Type;

            Helpers.ShowEvents(this, Engine.Base.Query.CurrentEvents());
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
            if (Kind != GH_ParamKind.input)
            {
                Menu_AppendSeparator(menu);
                Menu_AppendItem(menu, "Force preview", OnForcePreviewClick, true, m_ForcePreview);
                Menu_AppendItem(menu, "Max items to preview");
                Menu_AppendTextItem(menu, m_MaxItemsPreview.ToString(), null, OnMaxItemsPreviewChange, false);
            }

        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        protected int m_MaxItemsPreview = 10000;

        protected bool m_ForcePreview = false;

        /***************************************************/
    }
}



