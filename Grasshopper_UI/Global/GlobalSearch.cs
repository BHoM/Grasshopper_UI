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
using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using BH.Engine.Reflection;
using System.Diagnostics;

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

        private static void Instances_CanvasCreated(GH_Canvas canvas)
        {
            GlobalSearch.Activate(canvas.FindForm());
            GlobalSearch.ItemSelected += GlobalSearch_ItemSelected;

            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseUp += Canvas_MouseUp;
        }

        /*******************************************/

        private static void Canvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            GH_Canvas canvas = sender as GH_Canvas;
            if (canvas == null)
                return;

            GH_WireInteraction wire = canvas.ActiveInteraction as GH_WireInteraction;
            if (wire != null)
            {
                // Get source
                FieldInfo sourceField = typeof(GH_WireInteraction).GetField("m_source", BindingFlags.NonPublic | BindingFlags.Instance);
                if (sourceField == null)
                    return;
                IGH_Param sourceParam = sourceField.GetValue(wire) as IGH_Param;

                // Get the source Type
                Type sourceType = GetSourceType(sourceParam);
                if (sourceType == null)
                    return;

                // Get IsInput
                FieldInfo inputField = typeof(GH_WireInteraction).GetField("m_dragfrominput", BindingFlags.NonPublic | BindingFlags.Instance);
                if (inputField == null)
                    return;
                bool isInput = (bool)inputField.GetValue(wire);

                // Save wire info
                m_LastWire = new WireInfo
                {
                    Wire = wire,
                    Source = sourceParam,
                    SourceType = sourceType,
                    IsInput = isInput
                };

                Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "- Created wire info.");
            }            
        }

        /*******************************************/

        private static void Canvas_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            GH_Canvas canvas = sender as GH_Canvas;
            if (canvas == null)
                return;

            if (m_LastWire == null)
                return;

            FieldInfo targetField = typeof(GH_WireInteraction).GetField("m_target", BindingFlags.NonPublic | BindingFlags.Instance);
            if (targetField != null && targetField.GetValue(m_LastWire.Wire) != null)
            {
                m_LastWire = null;
                Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "- Wire info cleared.");
            }
            else
                GlobalSearch.Open(canvas.FindForm(), m_LastWire.SourceType, m_LastWire.IsInput);
        }

        /*******************************************/

        private static void GlobalSearch_ItemSelected(object sender, oM.UI.ComponentRequest request)
        {
            try
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
                    System.Drawing.PointF location = canvas.CursorCanvasPosition;
                    if (request != null && request.Location != null)
                        location = new System.Drawing.Point((int)request.Location.X, (int)request.Location.Y);
                    canvas.InstantiateNewObject(node.Id, initCode, canvas.CursorCanvasPosition, true);

                    if (m_LastWire != null && m_LastWire.Source != null)
                    {
                        GH_Component component = canvas.Document.Objects.Last() as GH_Component;
                        if (component != null)
                            Connect(component, m_LastWire);
                        else
                        {
                            IGH_Param param = canvas.Document.Objects.Last() as IGH_Param;
                            Connect(param, m_LastWire);
                        }

                        canvas.Invalidate();
                        if (component != null)
                            component.ExpireSolution(true);
                    }
                }

                Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "- Item Selected : " + ((request != null && request.CallerType != null) ? request.CallerType.Name : "null") + ((m_LastWire != null) ? ". Used wire." : ""));
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "- Error with item selection : " + e.Message);
            }

            m_LastWire = null;
        }

        /*******************************************/

        private static void Connect(GH_Component component, WireInfo wire)
        {
            if (component != null && wire != null && wire.SourceType != null)
            {
                if (wire.IsInput)
                {
                    IGH_Param param = component.Params.Output.FirstOrDefault(x => GetSourceType(x) == wire.SourceType);
                    if (param == null)
                        param = component.Params.Output.FirstOrDefault(x => wire.SourceType.IsAssignableFrom(GetSourceType(x)));
                    if (param != null)
                        wire.Source.AddSource(param);
                }
                else
                {
                    IGH_Param param = component.Params.Input.FirstOrDefault(x => GetSourceType(x) == wire.SourceType);
                    if (param == null)
                        param = component.Params.Input.FirstOrDefault(x =>
                        {
                            Type sourceType = GetSourceType(x);
                            return sourceType != null && GetSourceType(x).IsAssignableFrom(wire.SourceType);
                        });
                    if (param != null)
                        param.AddSource(wire.Source);
                }
            }
        }

        /*******************************************/

        private static void Connect(IGH_Param param, WireInfo wire)
        {
            if (param != null && wire != null && wire.SourceType != null)
            {
                if (wire.IsInput)
                    wire.Source.AddSource(param);
                else
                    param.AddSource(wire.Source);
            }
        }

        /*******************************************/

        private static Type GetSourceType(IGH_Param param)
        {
            Type sourceType = null;
            if (param is IBHoMParam)
                sourceType = ((IBHoMParam)param).ObjectType;
            else if (param is CallerValueList)
            {
                MultiChoiceCaller caller = ((CallerValueList)param).Caller;
                if (caller != null)
                    sourceType = caller.SelectedItem as Type;
            }

            if (sourceType == null)
                return null;
            else
                return sourceType.UnderlyingType().Type;
        }

        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private static WireInfo m_LastWire = null;


        /*******************************************/
    }
}

