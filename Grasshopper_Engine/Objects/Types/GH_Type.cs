using Grasshopper.Kernel.Types;
using System;

namespace BH.Engine.Alligator.Objects
{
    public class GH_Type : GH_BHoMGoo<Type>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Type";

        public override string TypeDescription { get; } = "Defines an object Type";

        public override bool IsValid { get { return m_value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_Type() : base() { }

        /***************************************************/

        public GH_Type(Type val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Type { Value = Value };
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            else if (source is string)
                this.Value = BH.Engine.Reflection.Create.Type(source as string);
            else if (source is GH_String)
                this.Value = BH.Engine.Reflection.Create.Type(((GH_String)source).Value);
            else if (source.GetType() == typeof(GH_Goo<Type>))
                this.Value = (Type)source;
            else
                this.Value = (Type)source;
            return true;
        }

        /***************************************************/

        public override string ToString()
        {
            Type val = Value;
            if (val == null)
                return "Undefined type";
            else
                return val.FullName;
        }

        /***************************************************/
    }
}
