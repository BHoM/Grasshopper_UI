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

using BH.Engine.Rhinoceros;
using BHG = BH.oM.Geometry;
using RHG = Rhino.Geometry;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using BH.Engine.Base;


namespace BH.UI.Grasshopper
{
    public static partial class Render
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static bool IRenderBHoMObject(this BH.oM.Base.IBHoMObject bhObject, GH_PreviewWireArgs args)
        {
            if (bhObject == null) { return false; }
            args.Pipeline.ZBiasMode = 0;
            Color bhColour = GetBHColor(args.Color);
            try
            {
                return RenderBHoMObject(bhObject as dynamic, args, bhColour);
            }
            catch (Exception) { }
            return false;
        }


        /***************************************************/
        /**** Public Methods  - Structural              ****/
        /***************************************************/

        //public static bool RenderBHoMObject(BH.oM.Structure.Elements.Node node, GH_PreviewWireArgs args, Color bhColour)
        //{
        //    BHG.Point pt = node.Position;
        //    RHG.Point3d location = pt.ToRhino().ToScreen(args);
        //    RenderBHoMGeometry(pt, args.Pipeline, bhColour);
        //    args.Pipeline.Draw2dText(node.Name, Color.Navy, location, false, 10);
        //    return true;
        //}

        /***************************************************/
        /**** Public Methods  - Acoustic                ****/
        /***************************************************/

        //public static bool RenderBHoMObject(BH.oM.Acoustic.Speaker speaker, GH_PreviewWireArgs args, Color bhColour)
        //{
        //    BHG.Point pt = speaker.Location;
        //    RHG.Point3d location = pt.ToRhino().ToScreen(args);
        //    RenderBHoMGeometry(pt, args.Pipeline, bhColour);
        //    args.Pipeline.Draw2dText(speaker.SpeakerID.ToString(), Color.Navy, location, false, 10);
        //    return true;
        //}

        /***************************************************/

        //public static bool RenderBHoMObject(BH.oM.Acoustic.Receiver receiver, GH_PreviewWireArgs args, Color bhColour)
        //{
        //    BHG.Point pt = receiver.Location;
        //    RHG.Point3d location = pt.ToRhino();
        //    RenderBHoMGeometry(pt, args.Pipeline, bhColour);
        //    args.Pipeline.Draw2dText(receiver.ReceiverID.ToString(), Color.Navy, location, false, 10);
        //    return true;
        //}

        /***************************************************/
        /**** Private Methods  -  Default case          ****/
        /***************************************************/

        private static bool RenderBHoMObject(BH.oM.Base.BHoMObject obj, GH_PreviewWireArgs args, Color bhColour)
        {
            IRenderBHoMGeometry(obj.IGeometry(), args);
            return true;
        }


        /***************************************************/
        /**** Private Methods  -                        ****/
        /***************************************************/

        //private static RHG.Point3d ToScreen(this RHG.Point3d point, GH_PreviewWireArgs args)
        //{
        //    RHG.Point2d clientPoint = args.Viewport.WorldToClient(point);
        //    RHG.Line pointRay = args.Viewport.ClientToWorld(clientPoint);
        //    RHG.Plane nearPlane;
        //    args.Viewport.GetFrustumNearPlane(out nearPlane);
        //    double t;
        //    Rhino.Geometry.Intersect.Intersection.LinePlane(pointRay, nearPlane, out t);
        //    return pointRay.PointAt(t - 0.01);
        //}

        /***************************************************/
    }
}
