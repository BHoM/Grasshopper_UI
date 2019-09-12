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
using BH.Engine.Base;
using BH.Engine.Rhinoceros;
using System;

namespace BH.Engine.Grasshopper.Objects
{
    public class IObjectGoo : BHGoo<IObject>, IGH_PreviewData
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public virtual Rhino.Geometry.BoundingBox ClippingBox { get { return Bounds(); } }

        public virtual Rhino.Geometry.BoundingBox Boundingbox { get { return Bounds(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public IObjectGoo()
        {
            this.Value = null;
        }

        /***************************************************/

        public IObjectGoo(IObject val)
        {
            this.Value = val;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool CastFrom(object source)
        {
            if (source == null)
                return true;

            while (source is IGH_Goo)
                source = ((IGH_Goo)source).ScriptVariable();

            if (source.GetType().Namespace.StartsWith("Rhino.Geometry"))
                source = BH.Engine.Rhinoceros.Convert.ToBHoM(source as dynamic);

            if (base.CastFrom(source))
            {
                return true;
            }

            return false;
        }

        /***************************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                return Engine.Grasshopper.Convert.ToGoo<Q>(Value, ref target);
            }
            catch (Exception e)
            {
                return !BH.Engine.Reflection.Compute.RecordError($"Cannot convert {Value.GetType().FullName} to {typeof(Q).FullName}.\n" +
                                                                 $"Inner Exception: {e.Message}");
            }
        }

        /***************************************************/

        protected override bool SetGeometry()
        {
            if (Value == null)
            {
                return true;
            }
            else if (Value is BHoMObject)
            {
                m_Geometry = ((BHoMObject)Value).IGeometry();
                m_RhinoGeometry = m_Geometry.IToRhino();
                return true;
            }
            else if (Value is IGeometry)
            {
                m_Geometry = Value as IGeometry;
                m_RhinoGeometry = m_Geometry.IToRhino();
                return true;
            }
            else
            {
                return false;
            }
        }

        /***************************************************/
    }
}
