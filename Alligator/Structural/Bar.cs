using BHoM.Structural;
using System;

namespace Alligator.Components
{
    public class CreateBar : BHoMBaseComponent<Bar>
    {
        public CreateBar() : base("Create Bar", "CreateBar", "Create a BHoM bar object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E50-410F-BBC7-C255FD0BD2B3");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.bar; }
        }
    }  
    
}
