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
using System.Windows.Forms;
using GH_IO.Serialization;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Parameters.Hints;
using Grasshopper.Kernel.Types;
using BH.Engine.Grasshopper.Objects;
using System.Runtime.CompilerServices;
using BH.oM.Base;

namespace BH.UI.Grasshopper.Parameters
{
    public class Param_Variable : BakeableParam<IGH_Goo>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.Variable_Param;

        public override Guid ComponentGuid { get; } = new Guid("D67B6CF3-37EA-438E-A6B4-2CC76B572658");

        public override string TypeName { get; } = "VariableObject";

        public override string InstanceDescription
        {
            get
            {
                if (SelectedHint == null)
                    return base.InstanceDescription;
                else
                    return $"{{Type hint: {SelectedHint.TypeName}}}" + Environment.NewLine + base.InstanceDescription;
            }
        }

        public IGH_TypeHint SelectedHint { get; set; } = new GH_NullHint();

        public List<IGH_TypeHint> PossibleHints { get; set; } = new List<IGH_TypeHint>();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_Variable() : base("Variable Object", "VariableObject", "Represents a collection of generic object with type that can be defined by  the user", "Params", "Primitive")
        {
            PossibleHints = Engine.Grasshopper.Query.AvailableHints;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            if (Kind != GH_ParamKind.output)
            {
                GH_DocumentObject.Menu_AppendItem(menu, "Item Access", ItemAccessClicked, Resources.ItemAccess, true, Access == GH_ParamAccess.item);
                GH_DocumentObject.Menu_AppendItem(menu, "List Access", ListAccessClicked, Resources.ListAccess, true, Access == GH_ParamAccess.list);
                GH_DocumentObject.Menu_AppendItem(menu, "Tree Access", TreeAccessClicked, Resources.TreeAccess, true, Access == GH_ParamAccess.tree);

                AppendMenuHintItems(menu);
            }
        }

        /*******************************************/

        public override bool Write(GH_IWriter writer)
        {
            bool rc = base.Write(writer);
            if (SelectedHint != null)
                writer.SetGuid("TypeHintID", SelectedHint.HintID);
            writer.SetInt32("ScriptParamAccess", (int)Access);
            return rc;
        }

        /*******************************************/

        protected override IGH_Goo PreferredCast(object data)
        {
            if (data is IObject)
                return new GH_Variable(data);
            else
            {
                IGH_Goo goo = GH_Convert.ToGoo(RuntimeHelpers.GetObjectValue(data));
                if (goo != null)
                    return goo;
                else
                    return null;
            }
        }

        /*******************************************/

        public override bool Read(GH_IReader reader)
        {
            bool rc = base.Read(reader);

            SelectedHint = null;
            if (reader.ItemExists("TypeHintID"))
            {
                try
                {
                    SelectedHint = GH_TypeHintServer.FindHintByID(reader.GetGuid("TypeHintID"));
                }
                catch { }
            }
                 
            if (reader.ItemExists("ScriptParamAccess"))
            {
                try
                {
                    Access = (GH_ParamAccess)reader.GetInt32("ScriptParamAccess");
                }
                catch { }
            }
            return rc;
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void ItemAccessClicked(object sender, EventArgs e)
        {
            if (Access != 0)
            {
                Access = GH_ParamAccess.item;
                OnObjectChanged(GH_ObjectEventType.DataMapping);
                ExpireSolution(true);
            }
        }

        /*******************************************/

        private void ListAccessClicked(object sender, EventArgs e)
        {
            if (Access != GH_ParamAccess.list)
            {
                Access = GH_ParamAccess.list;
                OnObjectChanged(GH_ObjectEventType.DataMapping);
                ExpireSolution(true);
            }
        }

        /*******************************************/

        private void TreeAccessClicked(object sender, EventArgs e)
        {
            if (Access != GH_ParamAccess.tree)
            {
                Access = GH_ParamAccess.tree;
                OnObjectChanged(GH_ObjectEventType.DataMapping);
                ExpireSolution(true);
            }
        }

        /*******************************************/

        private void TypeSelected(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (SelectedHint != item.Tag)
            {
                SelectedHint = (IGH_TypeHint)item.Tag;
                OnObjectChanged("hint_changed", SelectedHint);
                ExpireSolution(true);
            }
        }

        /*******************************************/

        private void AppendMenuHintItems(ToolStripDropDown menu)
        {
            ToolStripMenuItem hintItem = GH_DocumentObject.Menu_AppendItem(menu, "Type hint");
            foreach (IGH_TypeHint type in PossibleHints)
            {
                if (type != null)
                {
                    if (type is GH_HintSeparator)
                    {
                        GH_DocumentObject.Menu_AppendSeparator(hintItem.DropDown);
                    }
                    else if (!string.IsNullOrEmpty(type.TypeName))
                    {
                        bool @checked = false;
                        if (SelectedHint != null)
                        {
                            @checked = type.HintID == SelectedHint.HintID;
                        }
                        ToolStripMenuItem item = GH_DocumentObject.Menu_AppendItem(hintItem.DropDown, type.TypeName, TypeSelected, true, @checked);
                        item.Tag = type;
                    }
                }
            }
        }

        /*******************************************/
    }
}

