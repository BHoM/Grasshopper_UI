using Grasshopper.GUI.Canvas;
using System;
using System.IO;
using System.Reflection;

namespace BH.UI.Alligator.Menus
{
    public static class SearchMenu
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static void Activate()
        {
            Grasshopper.Instances.CanvasCreated += Instances_CanvasCreated;
        }


        /*******************************************/
        /**** Event Methods                     ****/
        /*******************************************/

        public static void Instances_CanvasCreated(GH_Canvas canvas)
        {
            string folder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Grasshopper\Libraries\Alligator\";
            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith("oM.dll") || file.EndsWith("Engine.dll"))
                    Assembly.LoadFrom(file);
            }

            canvas.KeyDown += ActiveCanvas_KeyDown;
        }

        /*******************************************/

        public static void ActiveCanvas_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            bool modCtrl = (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) == System.Windows.Forms.Keys.Control;

            if (e.KeyCode == System.Windows.Forms.Keys.B && modCtrl)
            {
                GH_Canvas canvas = Grasshopper.Instances.ActiveCanvas;
                System.Drawing.Point position = System.Windows.Forms.Cursor.Position;

                new BH_PopupSearchDialog { BasePoint = position, Canvas = canvas }.Show(canvas.FindForm());
            }
        }

        /*******************************************/
    }
}
