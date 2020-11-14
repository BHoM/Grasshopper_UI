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
using Rhino.Geometry;
using System.Drawing;
using BH.Engine.Reflection;

namespace BH.UI.Grasshopper.Goos
{
    public class GH_Variable : GH_BakeableObject<object>, IGH_QuickCast
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Variable Object";

        public override string TypeDescription { get; } = "Contains a generic object with a selectable type";

        public override bool IsValid { get { return Value != null; } }

        public GH_QuickCastType QC_Type { get { return GetQuickCastType(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_Variable() : base() { }

        /***************************************************/

        public GH_Variable(object val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Variable { Value = Value };
        }

        /*******************************************/

        public double QC_Distance(IGH_QuickCast other)
        {
            Point3d a = QC_Pt();
            Point3d b = other.QC_Pt();

            return a.DistanceTo(b);
        }

        /*******************************************/

        public int QC_Hash()
        {
            return Value.GetHashCode();
        }

        /*******************************************/

        public bool QC_Bool()
        {
            return (bool)Value; 
        }

        /*******************************************/

        public int QC_Int()
        {
            return (int)Value;
        }

        /*******************************************/

        public double QC_Num()
        {
            return (double)Value;
        }

        /*******************************************/

        public string QC_Text()
        {
            return Value.IToText();
        }

        /*******************************************/

        public Color QC_Col()
        {
            return (Color)Value;
        }

        /*******************************************/

        public Point3d QC_Pt()
        {
            if (Value is Point3d)
                return (Point3d)Value;
            else if (Value is oM.Geometry.Point)
                return ((oM.Geometry.Point)Value).ToRhino();
            else
            {
                string message = string.Format("Impossible to convert {0} into Rhino.Geometry.Point3d. Check the description of each input for more details on the type of object that need to be provided", Value.GetType().FullName);
                BH.Engine.Reflection.Compute.RecordError(message);
                return new Point3d();
            }
        }

        /*******************************************/

        public Vector3d QC_Vec()
        {
            if (Value is Vector3d)
                return (Vector3d)Value;
            else if (Value is oM.Geometry.Vector)
                return ((oM.Geometry.Vector)Value).ToRhino();
            else
            {
                string message = string.Format("Impossible to convert {0} into Rhino.Geometry.Vector3d. Check the description of each input for more details on the type of object that need to be provided", Value.GetType().FullName);
                BH.Engine.Reflection.Compute.RecordError(message);
                return new Vector3d();
            }
        }

        /*******************************************/

        public Complex QC_Complex()
        {
            return (Complex)Value;
        }

        /*******************************************/

        public Matrix QC_Matrix()
        {
            if (Value is Matrix)
                return (Matrix)Value;
            else
            {
                string message = string.Format("Impossible to convert {0} into Rhino.Geometry.Vector3d. Check the description of each input for more details on the type of object that need to be provided", Value.GetType().FullName);
                BH.Engine.Reflection.Compute.RecordError(message);
                return new Matrix(0,0);
            }
        }

        /*******************************************/

        public Interval QC_Interval()
        {
            return (Interval)Value;
        }

        /*******************************************/

        public int QC_CompareTo(IGH_QuickCast other)
        {
            double a = QC_Num();
            double b = other.QC_Num();

            return a.CompareTo(b);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private GH_QuickCastType GetQuickCastType()
        {
            if (Value is bool)
                return GH_QuickCastType.@bool;
            else if (Value is int)
                return GH_QuickCastType.@int;
            else if (Value is double || Value is float || Value is decimal)
                return GH_QuickCastType.num;
            else if (Value is Color)
                return GH_QuickCastType.col;
            else if (Value is Point3d || Value is oM.Geometry.Point)
                return GH_QuickCastType.pt;
            else if (Value is Vector3d || Value is oM.Geometry.Vector)
                return GH_QuickCastType.vec;
            else if (Value is Complex)
                return GH_QuickCastType.complex;
            else if (Value is Interval)
                return GH_QuickCastType.interval;
            else if (Value is Matrix || Value is oM.Geometry.TransformMatrix)
                return GH_QuickCastType.matrix;
            else
                return GH_QuickCastType.text;
        }

        /***************************************************/
    }
}

