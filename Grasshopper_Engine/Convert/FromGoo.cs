using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static T FromGoo<T>(this IGH_Goo goo)
        {
            if (goo == null)
                return default(T);

            // Get the data out of the Goo
            object data = goo.ScriptVariable();
            while (data is IGH_Goo)
                data = ((IGH_Goo)data).ScriptVariable();

            if (data == null)
                return default(T);

            // Convert the data to an acceptable format
            if (data is T)
                return (T)data;
            else
            {
                if (data.GetType().Namespace.StartsWith("Rhino.Geometry"))
                    data = BH.Engine.Rhinoceros.Convert.ToBHoM(data as dynamic);
                return (T)(data as dynamic);
            }
        }

        /*************************************/
    }
}
