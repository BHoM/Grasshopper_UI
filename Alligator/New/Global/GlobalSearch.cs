using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.UI.Alligator.Components;
using BH.UI.Alligator.Templates;
using BH.UI.Global;
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
            CallerComponent node = null;

            switch (request.CallerType.Name)
            {
                case "ComputeCaller":
                    node = new ComputeComponent();
                    break;
                case "ConvertCaller":
                    node = new ConvertComponent();
                    break;
                case "CreateObjectCaller":
                    node = new CreateObjectComponent();
                    break;
                case "ModifyCaller":
                    node = new ModifyComponent();
                    break;
                case "QueryCaller":
                    node = new QueryComponent();
                    break;
            }

            if (node != null)
            {
                GH_Canvas canvas = Grasshopper.Instances.ActiveCanvas;
                MethodBase method = request.SelectedItem as MethodBase;
                canvas.InstantiateNewObject(node.ComponentGuid, method.ToJson(), canvas.CursorCanvasPosition, true);
            }

        }

        /*******************************************/
    }
}
