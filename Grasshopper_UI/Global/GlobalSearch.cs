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
using Grasshopper.GUI.Ribbon;
using BH.oM.UI;
using Grasshopper.Kernel.Types;

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

            GlobalSearch.RemoveHandler(typeof(GlobalSearchMenu).FullName);
            GlobalSearch.ItemSelected += GlobalSearch_ItemSelected;
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static void Instances_CanvasCreated(GH_Canvas canvas)
        {
            GlobalSearch.Activate(canvas.FindForm());
            
            if (GH.Instances.DocumentEditor != null)
                GH.Instances.DocumentEditor.Activated += (sender, e) => AddLocalComponentProxies();

            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.KeyDown += Canvas_KeyDown;
        }

        /*******************************************/

        private static void Canvas_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (m_LastWire == null)
                return;

            if (e.KeyCode == System.Windows.Forms.Keys.Escape)
                m_LastWire = null;
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

                try
                {
                    IGH_DocumentObject docObject = sourceParam.Attributes.GetTopLevel.DocObject;
                    m_LastWire.Tags = new HashSet<string> { docObject.Category, docObject.SubCategory };
                }
                catch { }

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
            {
                GlobalSearch.Open(canvas.FindForm(), new SearchConfig
                {
                    TypeConstraint = m_LastWire.SourceType,
                    IsReturnType = m_LastWire.IsInput,
                    Tags = m_LastWire.Tags
                });
            } 
        }

        /*******************************************/

        private static void GlobalSearch_ItemSelected(object sender, oM.UI.ComponentRequest request)
        {
            try
            {
                GH_Canvas canvas = GH.Instances.ActiveCanvas;

                System.Drawing.PointF location = canvas.CursorCanvasPosition;
                if (request != null && request.Location != null)
                    location = new System.Drawing.Point((int)request.Location.X, (int)request.Location.Y);

                Caller node = null;
                if (request != null && request.CallerType != null)
                    node = Activator.CreateInstance(request.CallerType) as Caller;

                bool componentCreated = false;

                if (node != null)
                {
                    string initCode = "";
                    if (request.SelectedItem != null)
                        initCode = request.SelectedItem.ToJson();

                    componentCreated = canvas.InstantiateNewObject(node.Id, initCode, canvas.CursorCanvasPosition, true);
                }
                else if (request.SelectedItem is CustomItem)
                {
                    CustomItem item = request.SelectedItem as CustomItem;
                    if (item != null && item.Content is IGH_ObjectProxy)
                    {
                        IGH_ObjectProxy proxy = item.Content as IGH_ObjectProxy;
                        componentCreated = canvas.InstantiateNewObject(proxy.Guid, canvas.CursorCanvasPosition, true);
                    }
                    
                }

                if (componentCreated && m_LastWire != null && m_LastWire.Source != null)
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
                    if (param == null)
                        param = component.Params.Output.FirstOrDefault(x => GetSourceType(x) == typeof(object));
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
            else if (param.Type != null)
            {
                Type type = param.Type;
                if (type.BaseType != null && type.BaseType.IsGenericType)
                {
                    Type[] generics = type.BaseType.GetGenericArguments();
                    if (generics.Length > 0 && !generics[0].IsGenericType)
                        sourceType = generics[0];
                }
                else if (type == typeof(GH_Point))
                    sourceType = typeof(Rhino.Geometry.Point3d);
            }

            if (sourceType == null)
                return typeof(object);
            else
                return sourceType.UnderlyingType().Type;
        }

        /*******************************************/

        private static void AddLocalComponentProxies()
        {
            if (m_AddedLocalProxies)
                return;
            m_AddedLocalProxies = true;

            try
            {
                FieldInfo prop = typeof(GH.GUI.GH_DocumentEditor).GetField("_Ribbon", BindingFlags.NonPublic | BindingFlags.Instance);
                GH_Ribbon ribbon = prop.GetValue(GH.Instances.DocumentEditor) as GH_Ribbon;
                if (ribbon == null)
                    return;

                List<SearchItem> items = new List<SearchItem>();

                foreach (GH_RibbonTab tab in ribbon.Tabs)
                {
                    foreach (GH_RibbonPanel panel in tab.Panels)
                    {
                        foreach (IGH_ObjectProxy proxy in panel.AllItems.Select(x => x.Proxy))
                        {
                            try
                            {
                                object instance = Activator.CreateInstance(proxy.Type);
                                CustomItem item = new CustomItem { Content = proxy, Tags = new HashSet<string> { tab.NameFull, panel.Name } };

                                GH_Component component = instance as GH_Component;
                                IGH_Param param = instance as IGH_Param;

                                if (component != null && component.Params != null)
                                {
                                    if (component.Params.Input.Count > 0)
                                        item.InputTypes = component.Params.Input.Select(x => GetSourceType(x)).ToList();
                                    if (component.Params.Output.Count > 0)
                                        item.OutputTypes = component.Params.Output.Select(x => GetSourceType(x)).ToList();
                                }
                                else if (param != null)
                                {
                                    Type sourceType = GetSourceType(param);
                                    item.InputTypes.Add(sourceType);
                                    item.OutputTypes.Add(sourceType);
                                }

                                items.Add(new SearchItem
                                {
                                    CallerType = null,
                                    Item = item,
                                    Icon = proxy.Icon,
                                    Text = proxy.Desc != null ? proxy.Desc.Name + " (" + tab.NameFull + " - " + panel.Name + ")" : proxy.Type.Name
                                });
                            }
                            catch { }
                        }
                    }
                }

                GlobalSearch.AddPossibleItems(items);
            }
            catch { }

            return;
        }

        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private static WireInfo m_LastWire = null;
        private static bool m_AddedLocalProxies = false;


        /*******************************************/
    }
}

