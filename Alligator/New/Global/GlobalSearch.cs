using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.UI.Alligator.Components;
using BH.UI.Alligator.Templates;
using BH.UI.Global;
using BH.UI.Templates;
using Grasshopper.GUI.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Basilisk.Global
{
    public static class GlobalSearchMenu
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static void Activate()
        {
            Grasshopper.Instances.CanvasCreated += Instances_CanvasCreated;
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        public static void Instances_CanvasCreated(GH_Canvas canvas)
        {
            GlobalSearch.Activate(canvas.FindForm());
            GlobalSearch.ItemSelected += GlobalSearch_ItemSelected;

        }

        /*******************************************/

        private static void GlobalSearch_ItemSelected(object sender, oM.UI.ComponentRequest request)
        {
            Caller node = null;
            if (request != null && request.CallerType != null)
                node = Activator.CreateInstance(request.CallerType) as Caller;

            if (node != null)
            {
                string initCode = "";
                if (request.SelectedItem != null)
                    initCode = request.SelectedItem.ToJson();

                GH_Canvas canvas = Grasshopper.Instances.ActiveCanvas;
                canvas.InstantiateNewObject(node.Id, initCode, canvas.CursorCanvasPosition, true);
            }

        }

        /*******************************************/
    }
}
