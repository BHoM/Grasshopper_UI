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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Grasshopper.Others
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
                var errors = events.Where(x => x.Type == EventType.Error);
                var warnings = events.Where(x => x.Type == EventType.Warning);
                var notes = events.Where(x => x.Type == EventType.Note);

                foreach (Event e in errors)
                    component.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);

                foreach (Event e in warnings)
                    component.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, e.Message);

                GH_RuntimeMessageLevel noteLevel = GH_RuntimeMessageLevel.Remark;
                if (errors.Count() > 0 || warnings.Count() > 0)
                    noteLevel = GH_RuntimeMessageLevel.Blank;

                foreach (Event e in notes)
                    component.AddRuntimeMessage(noteLevel, e.Message);
            }
        }


        /*************************************/
    }
}
