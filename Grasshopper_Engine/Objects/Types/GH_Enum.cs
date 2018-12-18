using BH.Engine.Serialiser;
using GH_IO;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using System;

namespace BH.Engine.Grasshopper.Objects
{
    public class GH_Enum : GH_BHoMGoo<Enum>, GH_ISerializable
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Enum";

        public override string TypeDescription { get; } = "Defines an enum";

        public override bool IsValid { get { return Value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_Enum() : base() { }

        /***************************************************/

        public GH_Enum(Enum val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Enum { Value = Value };
        }

        /*******************************************/

        public override string ToString()
        {
            Enum val = Value;
            if (val == null)
                return "null";
            else
                return val.ToString();
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
                Value = (Enum)BH.Engine.Serialiser.Convert.FromJson(json);

            return true;
        }

        /***************************************************/

        public override bool Write(GH_IWriter writer)
        {
            if (Value != null)
                writer.SetString("Json", Value.ToJson());
            return true;
        }

        /*******************************************/
    }
}
