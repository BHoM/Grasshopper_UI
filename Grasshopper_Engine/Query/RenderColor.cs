using GH = Grasshopper;
using System.Drawing;

namespace BH.Engine.Grasshopper
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Color RenderColour(Color ghColour)
        {
            Color pColour = GH.Instances.ActiveCanvas.Document.PreviewColour;
            if (ghColour.R == pColour.R & // If the color sent by PreviewArgs is the default object PreviewColour
                ghColour.G == pColour.G &
                ghColour.B == pColour.B) // Excluding Alpha channel from comparison
            {
                return Color.FromArgb(80, 255, 41, 105);
            }
            else
            {
                return ghColour;
            }
        }

        /***************************************************/
    }
}
