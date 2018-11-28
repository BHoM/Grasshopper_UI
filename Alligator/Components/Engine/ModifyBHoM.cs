using System;
using Grasshopper.Kernel;
using System.Linq;
using BH.UI.Alligator.Templates;
using System.Reflection;
using BH.oM.DataStructure;
using System.Collections;

namespace BH.UI.Alligator.Base
{
    public class ModifyBHoM : MethodCallTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("C275B1A2-BB2D-4F3B-8D5C-18C78456A831"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Modify; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.secondary;

        public override string MethodGroup { get; set; } = "Modify";

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ModifyBHoM() : base("Modify BHoM Object", "ModifyBHoM", "Modify a BHoMObject", "Alligator", " Engine") {}


        /*******************************************/
    }
}