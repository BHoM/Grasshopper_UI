/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.GUI;
using BH.oM.Base;
using BH.UI.Grasshopper.Templates;
using BH.UI.Base;
using BH.UI.Base.Components;
using GH = Grasshopper;
using System.Windows.Forms;
using GH_IO.Serialization;
using BH.Engine.Reflection;
using BH.oM.UI;
using Grasshopper.GUI.RemotePanel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI.Canvas;
using Rhino.Runtime;
using BH.Engine.Base;
using BH.UI.Grasshopper.CustomAttributes;

namespace BH.UI.Grasshopper.Components
{
    public class FalseStartToggleComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new FalseStartToggleCaller();

        /*******************************************/

        public FalseStartToggleComponent()
        {

        }

        /*******************************************/

        public override bool Read(GH_IReader reader)
        {
            bool success = base.Read(reader);

            //Ensure the component is defaulting to false on load
            Caller.SetItem(false);

            // Adding a tag under the component
            bool value = (bool)Caller.SelectedItem;
            Message = value.ToString();

            return success;
        }

            /*******************************************/
            
        public override void CreateAttributes()
        {
            m_attributes = new FalseStartToggleAttributes(this);
        }

        /*******************************************/
            
        public void UpdateComponent()
        {
            (Caller as FalseStartToggleCaller).SetItem(!(Caller as FalseStartToggleCaller).Value);
        }

            /*******************************************/
            
        protected override void OnCallerModified(object sender, CallerUpdate update)
        {
            // Adding a tag under the component
            bool value = (bool)Caller.SelectedItem;

            Message = value.ToString();
            
            base.OnCallerModified(sender, update);
        }
    }
}



