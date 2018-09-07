using Grasshopper;
using Grasshopper.Kernel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Grasshopper
{
    public static partial class Create
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static DataTree<T> DataTree<T>(List<IEnumerable<T>> data, int iteration)
        {
            DataTree<T> tree = new DataTree<T>();

            if (data.Count == 0)
                tree.EnsurePath(new GH_Path(iteration));

            for (int i = 0; i < data.Count; i++)
            {
                tree.AddRange(data[i], new GH_Path(iteration, i));
            }

            return tree;
        }

        /*******************************************/
    }
}
