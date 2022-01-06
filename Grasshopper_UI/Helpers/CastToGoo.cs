/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Grasshopper
{
    public static partial class Helpers
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public static bool ICastToGoo(object value, ref object target)
        {
            return CastToGoo(target as dynamic, value as dynamic);
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Arc target)
        {
            return GH_Convert.ToGHArc(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Boolean target)
        {
            return GH_Convert.ToGHBoolean(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Box target)
        {
            return GH_Convert.ToGHBox(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Brep target)
        {
            return GH_Convert.ToGHBrep(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Circle target)
        {
            return GH_Convert.ToGHCircle(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Colour target)
        {
            return GH_Convert.ToGHColour(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_ComplexNumber target)
        {
            return GH_Convert.ToGHComplexNumber(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Curve target)
        {
            return GH_Convert.ToGHCurve(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Guid target)
        {
            return GH_Convert.ToGHGuid(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Integer target)
        {
            return GH_Convert.ToGHInteger(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Interval target)
        {
            return GH_Convert.ToGHInterval(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Line target)
        {
            return GH_Convert.ToGHLine(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Matrix target)
        {
            return GH_Convert.ToGHMatrix(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Mesh target)
        {
            return GH_Convert.ToGHMesh(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_MeshFace target)
        {
            return GH_Convert.ToGHMeshFace(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Number target)
        {
            return GH_Convert.ToGHNumber(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Plane target)
        {
            return GH_Convert.ToGHPlane(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Point target)
        {
            return GH_Convert.ToGHPoint(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Rectangle target)
        {
            return GH_Convert.ToGHRectangle(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_String target)
        {
            return GH_Convert.ToGHString(value.IToText(), GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Surface target)
        {
            return GH_Convert.ToGHSurface(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Time target)
        {
            return GH_Convert.ToGHTime(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Transform target)
        {
            try
            {
                target = new GH_Transform(value as dynamic);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref GH_Vector target)
        {
            return GH_Convert.ToGHVector(value, GH_Conversion.Both, ref target);
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref IGH_GeometricGoo target)
        {
            try
            {
                target = GH_Convert.ToGeometricGoo(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /*******************************************/

        public static bool CastToGoo(object value, ref IGH_QuickCast target)
        {
            try
            {
                if (value is bool)
                    target = new GH_Boolean((bool)value);
                else if (value is Color)
                    target = new GH_Colour((Color)value);
                else if (value is Complex)
                    target = new GH_ComplexNumber((Complex)value);
                else if (value is int)
                    target = new GH_Integer((int)value);
                else if (value is Interval)
                    target = new GH_Interval((Interval)value);
                else if (value is Matrix)
                    target = new GH_Matrix((Matrix)value);
                else if (value is double || value is float)
                    target = new GH_Number((double)value);
                else if (value is Point3d)
                    target = new GH_Point((Point3d)value);
                else if (value is string)
                    target = new GH_String(value.ToString());
                else if (value is Vector3d)
                    target = new GH_Vector((Vector3d)value);
                return true;
            }
            catch
            {
                return false;
            }
        }



        /*******************************************/
        /**** Fallback Methods                  ****/
        /*******************************************/

        public static bool CastToGoo<T>(object value, ref T target)
        {
            string message = string.Format("Could not find a conversion from {0} to {1}. Check the description of each input for more details on the type of object that need to be provided", value.GetType().FullName, typeof(T).FullName);
            BH.Engine.Reflection.Compute.RecordError(message);
            return false;
        }

        /*************************************/
    }
}



