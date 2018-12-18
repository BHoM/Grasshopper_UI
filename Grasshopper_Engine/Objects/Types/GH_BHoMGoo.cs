using Grasshopper.Kernel.Types;
using System;

namespace BH.Engine.Grasshopper.Objects
{
    public abstract class GH_BHoMGoo<T> : GH_Goo<T> 
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override bool IsValid { get { return Value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_BHoMGoo()
        {
            this.Value = default(T);
        }

        /***************************************************/

        public GH_BHoMGoo(T val)
        {
            this.Value = val;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override string ToString()
        {
            if (Value == null)
                return "null";
            return Value.ToString();
        }

        /*******************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                object ptr = this.Value;
                target = (Q)ptr;
                return true;
            }
            catch (Exception)
            {
                string message = string.Format("Impossible to convert {0} into {1}. Check the input description for more details on the type of object that need to be provided", Value.GetType().FullName, typeof(Q).FullName);
                throw new Exception(message);
            }
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            else if (source.GetType() == typeof(GH_Goo<T>))
                this.Value = ((GH_Goo<T>)source).Value;
            else if (source is T)
                this.Value = (T)source;
            else
                this.Value = default(T);
            return true;
        }

        /*******************************************/
    }
}
