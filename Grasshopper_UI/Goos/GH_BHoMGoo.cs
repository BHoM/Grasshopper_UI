/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using BH.Engine.Base;
using Grasshopper.Kernel.Types;
using System;

namespace BH.UI.Grasshopper.Goos
{
    public abstract class GH_BHoMGoo<T> : GH_Goo<T> 
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override bool IsValid { get { return Value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_BHoMGoo()
        {
            this.Value = default(T);
        }

        /***************************************************/

        public GH_BHoMGoo(T val)
        {
            this.Value = val;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override string ToString()
        {
            if (Value == null)
                return "null";
            return Value.IToText();
        }

        /*******************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                object ptr = this.Value;
                target = (Q)ptr;
                return true;
            }
            catch (Exception)
            {
                string message = string.Format("Impossible to convert {0} into {1}. Check the input description for more details on the type of object that need to be provided", Value.GetType().FullName, typeof(Q).FullName);
                Engine.Base.Compute.RecordError(message);
                return false;
            }
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            try
            {
                if(source == null)
                    return false; 
                else if (source.GetType() == typeof(GH_Goo<T>))
                    this.Value = ((GH_Goo<T>)source).Value;
                else
                    this.Value = (T)source;

                return true;
            }
            catch
            {
                string message = $"Impossible to convert {source.GetType().IToText()} into {typeof(T)}. Check the description of each input for more details on the type of object that need to be provided";
                BH.Engine.Base.Compute.RecordError(message);
                return false;
            } 
        }

        /*******************************************/
    }
}







