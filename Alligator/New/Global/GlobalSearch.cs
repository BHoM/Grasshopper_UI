using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.UI.Grasshopper.Components;
using BH.UI.Grasshopper.Templates;
using BH.UI.Global;
using BH.UI.Templates;
using Grasshopper.GUI.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GH = Grasshopper;

namespace BH.UI.Grasshopper.Global
{
    public static class GlobalSearchMenu
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static void Activate()
        {
            GH.Instances.CanvasCreated += Instances_CanvasCreated;
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

                GH_Canvas canvas = GH.Instances.ActiveCanvas;
                canvas.InstantiateNewObject(node.Id, initCode, canvas.CursorCanvasPosition, true);
            }

        }

        /*******************************************/
    }
}
