using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using BH.Engine.Base;
using BH.Adapter.Rhinoceros;
using Rhino;
using Rhino.DocObjects;

namespace BH.UI.Alligator
{
    public abstract class GH_TemplateType<T> : GH_Goo<T> 
    {
        /***************************************************/
        /**** Properties Override                       ****/
        /***************************************************/

        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return true;
            }
        }



        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public GH_TemplateType()
        {
            this.Value = default(T);
        }

        /***************************************************/

        public GH_TemplateType(T val)
        {
            this.Value = val;
        }


        /***************************************************/
        /**** Public Methods Override                   ****/
        /***************************************************/

        public override string ToString()
        {
            if (Value == null)
                return "null";
            return Value.ToString();
        }


        /***************************************************/
        /**** Automatic casting methods                 ****/
        /***************************************************/

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
            else
                this.Value = (T)source;
            return true;
        }

    }
}
