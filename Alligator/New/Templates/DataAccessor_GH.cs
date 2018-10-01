using BH.oM.Geometry;
using BH.UI.Templates;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Alligator.Templates
{
    public class DataAccessor_GH : DataAccessor
    {
        /*************************************/
        /**** Properties                  ****/
        /*************************************/

        public IGH_DataAccess GH_Accessor { get; set; } = null;


        /*************************************/
        /**** Input Getter Methods        ****/
        /*************************************/

        public override T GetDataItem<T>(int index)
        {
            if (GH_Accessor == null)
                return default(T);

            IGH_Goo goo = null;
            GH_Accessor.GetData(index, ref goo);
            return BH.Engine.Grasshopper.Convert.FromGoo<T>(goo);
        }

        /*************************************/

        public override List<T> GetDataList<T>(int index)
        {
            if (GH_Accessor == null)
                return new List<T>();

            List<IGH_Goo> goo = new List<IGH_Goo>();
            GH_Accessor.GetDataList<IGH_Goo>(index, goo);
            return goo.Select(x => BH.Engine.Grasshopper.Convert.FromGoo<T>(x)).ToList();
        }

        /*************************************/

        public override List<List<T>> GetDataTree<T>(int index)
        {
            if (GH_Accessor == null)
                return new List<List<T>>();

            GH_Structure<IGH_Goo> goo = new GH_Structure<IGH_Goo>();
            GH_Accessor.GetDataTree(index, out goo);
            return goo.Branches.Select(x => x.Select(y => BH.Engine.Grasshopper.Convert.FromGoo<T>(y)).ToList()).ToList();
        }

        /*************************************/
        /**** Output Setter Methods       ****/
        /*************************************/

        public override bool SetDataItem<T>(int index, T data)
        {
            if (data == null)
            {
                return GH_Accessor.SetData(index, data);
            }
            else if (typeof(IGeometry).IsAssignableFrom(data.GetType()))
            {
                object result = BH.Engine.Grasshopper.Convert.ToRhino(data);
                if (result is IEnumerable)
                    return GH_Accessor.SetDataList(index, result as IEnumerable);
                else
                    return GH_Accessor.SetData(index, result);
            }
            else
                return GH_Accessor.SetData(index, data);
        }

        /*************************************/

        public override bool SetDataList<T>(int index, IEnumerable<T> data)
        {
            if (data != null && typeof(IGeometry).IsAssignableFrom(typeof(T)))
                return GH_Accessor.SetDataList(index, ((IEnumerable<T>)data).Select(x => BH.Engine.Grasshopper.Convert.ToRhino(x)));
            else
                return GH_Accessor.SetDataList(index, (IEnumerable<T>)data);
        }

        /*************************************/

        public override bool SetDataTree<T>(int index, IEnumerable<IEnumerable<T>> data)
        {
            if (typeof(IGeometry).IsAssignableFrom(typeof(T)))
                return GH_Accessor.SetDataTree(index, BH.Engine.Grasshopper.Create.DataTree(((IEnumerable<IEnumerable<T>>)data).Select(v => v.Select(x => (BH.Engine.Grasshopper.Convert.ToRhino(x)))).ToList(), GH_Accessor.Iteration));
            else
                return GH_Accessor.SetDataTree(index, BH.Engine.Grasshopper.Create.DataTree(((IEnumerable<IEnumerable<T>>)data).ToList(), GH_Accessor.Iteration));
        }

        /*************************************/
    }
}
