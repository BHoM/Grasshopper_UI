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

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using BH.Engine.Base;
using BH.Engine.Rhinoceros;
using Rhino;
using Rhino.DocObjects;
using GH_IO;
using GH_IO.Serialization;
using BH.Engine.Serialiser;

namespace BH.Engine.Grasshopper.Objects
{
    public class BHoMObjectGoo : GH_BHoMGoo<IBHoMObject>, IGH_PreviewData, GH_ISerializable
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Bounds(); } }

        public Rhino.Geometry.BoundingBox Boundingbox { get { return Bounds(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMObjectGoo()
        {
            this.Value = null;
        }

        /***************************************************/

        public BHoMObjectGoo(BHoMObject val)
        {
            this.Value = val;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public virtual Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            Rhino.Geometry.BoundingBox box = Bounds();
            box.Transform(xform);
            return box;
        }

        /***************************************************/

        public override string ToString()
        {
            return Value?.ToString();
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
                Value = (BHoMObject)BH.Engine.Serialiser.Convert.FromJson(json);

            return true;
        }

        /***************************************************/

        public override bool Write(GH_IWriter writer)
        {
            if (Value != null)
                writer.SetString("Json", Value.ToJson());
            return true;
        }


        /***************************************************/
        /**** Private Method                            ****/
        /***************************************************/

        protected override bool SetGeometry()
        {
            if (Value == null)
                return true;

            m_Geometry = Value.IGeometry();
            if (m_Geometry == null)
                return true;

            m_RhinoGeometry = m_Geometry.IToRhino();

            return true;
        }

        /***************************************************/
    }
}
