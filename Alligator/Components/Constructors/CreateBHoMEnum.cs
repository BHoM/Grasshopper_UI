using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using BH.UI.Alligator.Templates;

namespace BH.UI.Alligator.Base
{
    public class CreateBHoMEnum : CreateEnumTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("68B29FAE-057B-417A-96BC-32224974CCBE"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Enum; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateBHoMEnum() : base("Create BHoM Enum", "BHoMEnum", "Creates a specific type of BHoM Enum", "Alligator", " oM")
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override IEnumerable<Type> GetRelevantEnums()
        {
            return BH.Engine.Reflection.Query.BHoMEnumList();
        }

        /*******************************************/
    }
}