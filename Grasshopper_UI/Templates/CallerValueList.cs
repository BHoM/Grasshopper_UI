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

using System;
using Rhino;
using Rhino.Commands;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.oM.UI;
using BH.Engine.Grasshopper;
using BH.Engine.Grasshopper.Objects;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Templates;
using System.Windows.Forms;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using BH.Engine.Reflection;

namespace BH.UI.Grasshopper.Templates
{
    public abstract class CallerValueList : GH_ValueList, IGH_InitCodeAware
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public abstract MultiChoiceCaller Caller { get; }

        protected override System.Drawing.Bitmap Icon { get { return Caller.Icon_24x24; } }

        public override Guid ComponentGuid { get { return Caller.Id; } }

        public override GH_Exposure Exposure { get { return (GH_Exposure)Math.Pow(2, Caller.GroupIndex); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CallerValueList() : base()
        { 
            Name = Caller.Name;
            NickName = Caller.Name;
            Description = Caller.Description;
            Category = "BHoM";
            SubCategory = Caller.Category;
            ListItems.Clear();

            m_Accessor = new DataAccessor_GH(new List<IGH_Param>());
            Caller.SetDataAccessor(m_Accessor);

            Caller.ItemSelected += (sender, e) => UpdateFromSelectedItem();
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public virtual void UpdateFromSelectedItem()
        {
            this.NickName = Caller.Name;
            this.Name = Caller.Name;
            this.Description = Caller.Description;

            ListItems.Clear();
            List<string> names = Caller.GetChoiceNames();
            for (int i = 0; i < names.Count; i++)
                ListItems.Add(new GH_ValueListItem(names[i], i.ToString()));

            this.ExpireSolution(true);
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void CollectVolatileData_Custom()
        {
            this.m_data.Clear();
            List<GH_ValueListItem>.Enumerator enumerator = this.SelectedItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GH_ValueListItem item = enumerator.Current;
                int index = 0;
                item.Value.CastTo<int>(out index);
                object result = Caller.Run(new object[] { index });
                this.m_data.Append(result.IToGoo(), new GH_Path(0));
            }

            Engine.UI.Compute.LogUsage("Grasshopper", InstanceGuid, Caller.SelectedItem);
        }

        /*******************************************/

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            GH_DocumentObject.Menu_AppendSeparator(menu);
            Caller.AddToMenu(menu);
        }

        /*******************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetString("Component", Caller.Write());

            int index = ListItems.IndexOf(FirstSelectedItem);
            if (index >= 0)
                writer.SetInt32("Selection", index);

            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (!base.Read(reader))
                return false;

            string callerString = ""; reader.TryGetString("Component", ref callerString);
            if (Caller.Read(callerString))
            {
                int selection = -1;
                reader.TryGetInt32("Selection", ref selection);

                if (selection >= 0 && selection < ListItems.Count)
                {
                    //To acount for changes in orders of enums and dataset, match name of previous item with name of new items instead of 
                    //simply relying on previous index.

                    //Get name of selected item
                    string prevName = ListItems[selection].Name;
                    //Find index in list of new items with matching name to selected name
                    int index = Caller.GetChoiceNames().IndexOf(prevName);
                    //Make sure component is up to date with backend caller
                    UpdateFromSelectedItem();

                    //If index is found, update to this. If item is not found, use previous selection.
                    if (index != -1)
                        selection = index;

                    //As item count might have been reduced in UpdateFromSelected, a final check that the selection is not out of bounds needed.
                    if (selection < ListItems.Count)
                        this.SelectItem(selection);
                }
                return true;
            }
            else
                return false;
        }
        
        /*************************************/

        protected override string HtmlHelp_Source()
        {
            if (Caller != null)
            {
                BH.Engine.Reflection.Compute.IOpenHelpPage(Caller.SelectedItem);
            }
            return base.HtmlHelp_Source();
        }

        /*************************************/
        /**** Initialisation via String   ****/
        /*************************************/

        public void SetInitCode(string code)
        {
            object item = BH.Engine.Serialiser.Convert.FromJson(code);
            if (item != null)
                Caller.SetItem(item);
            UpdateFromSelectedItem();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private DataAccessor_GH m_Accessor = null;


        /*******************************************/
    }
}

