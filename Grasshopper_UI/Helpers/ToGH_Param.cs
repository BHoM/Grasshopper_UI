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

using BH.oM.Reflection.Debugging;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.UI;
using BH.oM.Reflection;
using Grasshopper.Kernel.Parameters;
using BH.Engine.Reflection;
using BH.UI.Grasshopper.Parameters;
using BH.oM.Geometry;
using BH.oM.Base;
using System.Collections;
using BH.Adapter;
using BH.UI.Grasshopper.Templates;
using BH.Engine.Grasshopper;
using Grasshopper.Kernel.Parameters.Hints;

namespace BH.UI.Grasshopper
{
    public static partial class Helpers
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static IGH_Param ToGH_Param(this ParamInfo info)
        {
            UnderlyingType subType = info.DataType.UnderlyingType();
            IGH_Param param;

            switch (subType.Type.FullName)
            {
                case "System.Boolean":
                    param = new Param_Boolean();
                    break;
                case "System.Drawing.Color":
                    param = new Param_Colour();
                    break;
                case "System.DateTime":
                    param = new Param_Time();
                    break;
                case "System.Double":
                    param = new Param_Number();
                    break;
                case "System.Guid":
                    param = new Param_Guid();
                    break;
                case "System.Int16":
                case "System.Int32":
                    param = new Param_Integer();
                    break;
                case "System.Int64":
                    param = new Param_Time();
                    break;
                case "System.String":
                    param = new Param_String();
                    break;
                case "System.Type":
                    param = new Param_Type();
                    break;
                default:
                    Type type = subType.Type;
                    if (typeof(IGeometry).IsAssignableFrom(type))
                        param = new Param_BHoMGeometry();
                    else if (typeof(IBHoMObject).IsAssignableFrom(type))
                        param = new Param_BHoMObject();
                    else if (typeof(IObject).IsAssignableFrom(type))
                        param = new Param_IObject();
                    else if (typeof(Enum).IsAssignableFrom(type))
                        param = new Param_Enum();
                    else if (typeof(IDictionary).IsAssignableFrom(type))
                        param = new Param_Dictionary();
                    else if (typeof(BHoMAdapter).IsAssignableFrom(type))
                        param = new Param_BHoMAdapter();
                    else
                    {
                        param = new Param_Variable
                        {
                            SelectedHint = new GH_NullHint(),
                            PossibleHints = Engine.Grasshopper.Query.AvailableHints,
                        };
                    }
                        
                    break;
            }

            param.Access = (GH_ParamAccess)subType.Depth;
            param.Description = info.Description;
            param.Name = info.Name;
            param.NickName = info.Name;
            param.Optional = !info.IsRequired;

            //TODO: Is it necessary to react to param.AttributesChanged ?

            if (param is IBHoMParam)
                ((IBHoMParam)param).ObjectType = subType.Type;

            try
            {
                if (info.HasDefaultValue && !info.IsRequired)
                    ((dynamic)param).SetPersistentData(info.DefaultValue.IToGoo());
            }
            catch { }

            return param;
        }

        /*******************************************/
    }
}
