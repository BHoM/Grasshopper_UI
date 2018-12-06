using GH = Grasshopper;
using System.Drawing;
using Rhino.Display;

namespace BH.Engine.Alligator
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static DisplayMaterial RenderMaterial(DisplayMaterial material)
        {
            Color pColour = GH.Instances.ActiveCanvas.Document.PreviewColour;
            Color ghColour = material.Diffuse;
            if (ghColour.R == pColour.R & // If the color sent by PreviewArgs is the default object PreviewColour
                ghColour.G == pColour.G &
                ghColour.B == pColour.B) // Excluding Alpha channel from comparison
            {
                return new DisplayMaterial(Color.FromArgb(255, 255, 41, 105), 0.6);
            }
            else
            {
                return material;
            }
        }

        /***************************************************/
    }
}
