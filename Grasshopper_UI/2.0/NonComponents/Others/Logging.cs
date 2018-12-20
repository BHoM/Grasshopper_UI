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

using BH.oM.Reflection.Debugging;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Grasshopper
{
    public static class Logging
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static void ShowEvents(GH_ActiveObject component, List<Event> events)
        {
            if (events.Count > 0)
            {
                foreach (Event e in events)
                {
                    if (e.Type == EventType.Error)
                        component.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
                    else if (e.Type == EventType.Warning)
                        component.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, e.Message);
                    else if (e.Type == EventType.Note)
                        component.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, e.Message);
                }
            }
        }


        /*************************************/
    }
}
