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

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using BH.Engine.Rhinoceros;
using Rhino;
using Rhino.DocObjects;
using GH_IO.Serialization;
using BH.Engine.Serialiser;
using GH_IO;
using Rhino.Geometry;

namespace BH.UI.Grasshopper.Goos
{
    public class GH_IBHoMGeometry : GH_BakeableObject<IGeometry>, IGH_PreviewData, IGH_BakeAwareData, GH_ISerializable, IGH_GeometricGoo
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "IGeometry";

        public override string TypeDescription { get; } = "Contains a generic BHoM Geometry";

        public Guid ReferenceID { get; set; } = Guid.Empty;

        public bool IsReferencedGeometry { get; } = false;

        public bool IsGeometryLoaded { get; } = true;


        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public GH_IBHoMGeometry() : base() { }

        /***************************************************/

        public GH_IBHoMGeometry(IGeometry val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_IBHoMGeometry { Value = Value };
        }

        /*******************************************/

        public IGH_GeometricGoo DuplicateGeometry()
        {
            return new GH_IBHoMGeometry { Value = Value };
        }

        /*******************************************/

        public IGH_GeometricGoo Transform(Transform xform)
        {
            if (Value == null)
                return null;
            else
                return new GH_IBHoMGeometry { Value = Value.ITransform(xform.FromRhino()) };
        }

        /*******************************************/

        public IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            if (m_RhinoGeometry == null)
                return null;
            else if (m_RhinoGeometry is Point3d)
            {
                Point3d morphed = xmorph.MorphPoint((Rhino.Geometry.Point3d)m_RhinoGeometry);
                return new GH_IBHoMGeometry { Value = morphed.FromRhino() };
            } 
            else
            {
                GeometryBase geometry = ((GeometryBase)m_RhinoGeometry).Duplicate();
                xmorph.Morph(geometry);
                return new GH_IBHoMGeometry { Value = geometry?.FromRhino() };
            }
        }

        /*******************************************/

        public bool LoadGeometry()
        {
            return LoadGeometry(RhinoDoc.ActiveDoc);
        }

        /*******************************************/

        public bool LoadGeometry(RhinoDoc doc)
        {
            return true;
        }

        /*******************************************/

        public void ClearCaches()
        {
            
        }

        /***************************************************/
    }
}

