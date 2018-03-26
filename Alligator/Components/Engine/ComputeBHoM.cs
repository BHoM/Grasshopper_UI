using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using System.Reflection;
using BH.oM.DataStructure;
using System.Collections;
using BH.Engine.Reflection;
using BH.Engine.Reflection.Convert;
using BH.Engine.DataStructure;

namespace BH.UI.Alligator.Base
{
    public class ComputeBHoM : MethodCallTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("9A94F1C4-AF5B-48E6-B0DD-F56145DEEDDA"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Compute; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.secondary;

        public override string MethodGroup { get; set; } = "Compute";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ComputeBHoM() : base("Compute / Analyse", "ComputeBHoM", "Run a computationally intensive calculation", "Alligator", " Engine") {}

        /*******************************************/
    }
}