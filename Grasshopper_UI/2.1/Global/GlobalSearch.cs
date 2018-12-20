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

using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.UI.Grasshopper.Components;
using BH.UI.Grasshopper.Templates;
using BH.UI.Global;
using BH.UI.Templates;
using Grasshopper.GUI.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GH = Grasshopper;

namespace BH.UI.Grasshopper.Global
{
    public static class GlobalSearchMenu
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static void Activate()
        {
            GH.Instances.CanvasCreated += Instances_CanvasCreated;
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        public static void Instances_CanvasCreated(GH_Canvas canvas)
        {
            GlobalSearch.Activate(canvas.FindForm());
            GlobalSearch.ItemSelected += GlobalSearch_ItemSelected;

        }

        /*******************************************/

        private static void GlobalSearch_ItemSelected(object sender, oM.UI.ComponentRequest request)
        {
            Caller node = null;
            if (request != null && request.CallerType != null)
                node = Activator.CreateInstance(request.CallerType) as Caller;

            if (node != null)
            {
                string initCode = "";
                if (request.SelectedItem != null)
                    initCode = request.SelectedItem.ToJson();

                GH_Canvas canvas = GH.Instances.ActiveCanvas;
                canvas.InstantiateNewObject(node.Id, initCode, canvas.CursorCanvasPosition, true);
            }

        }

        /*******************************************/
    }
}
