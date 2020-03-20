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

                //If any warnings or errors have been added, then add the message as a blank.
                //This is to ensure the colour of the component gets the appropriate colour.
                //(A bug in GH ranks remarks higher than warning and errors, keeping the component grey when it should be orange/red)
                //Solution to use 'Blank' warning level, which generally does not do anything on the component.
                if (errors.Count() > 0 || warnings.Count() > 0)
                {
                    foreach (Event e in notes)
                        component.AddBlankNoteMessage(e.Message);
                }
                else
                {
                    foreach (Event e in notes)
                        component.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, e.Message);
                }
            }
        }

        /*************************************/
        /**** Private Methods             ****/
        /*************************************/

        private static void AddBlankNoteMessage(this GH_ActiveObject component, string text)
        {
            //Method added to be able to force in 'Blank' messages to the component.
            //Method added because  'Blank' messages are not added when `AddRuntimeMessage` is called.
            //We are adding 'Blank' messages because 'Remark' is treated with higher priority than any of the others (bug in core Grasshopper),
            //which is causing issues with component colouring.
            //This is, the component stays standard grey, even if an error or warning has been raised.

            //If this bug is fixed, this method can be removed and we can return to only adding remarks to the components instead.

            var messages = component.RuntimeMessages(GH_RuntimeMessageLevel.Blank);

            foreach (string message in messages)
            {
                if (message == text)
                    return;
            }

            try
            {
                //Force add blank messages via reflection
                //The code below is executing: 'm_messages.Add(new Message(text, GH_RuntimeMessageLevel.Blank));'
                Type t = typeof(GH_ActiveObject);
                FieldInfo messageField = t.GetField("m_messages", BindingFlags.NonPublic | BindingFlags.Instance);
                Type messageType = messageField.FieldType.GenericTypeArguments[0];
                ConstructorInfo constructor = messageType.GetConstructors().First();

                var mess = constructor.Invoke(new object[] { text, GH_RuntimeMessageLevel.Blank });

                var messageList = messageField.GetValue(component);
                MethodInfo addMethod = messageField.FieldType.GetMethod("Add");
                addMethod.Invoke(messageList, new object[] { mess });
            }
            catch (Exception)
            {
                //As we are already in the message handling, no real place to add messages here
            }


        }

        /*************************************/
    }
}
