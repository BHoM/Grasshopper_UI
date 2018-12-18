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
