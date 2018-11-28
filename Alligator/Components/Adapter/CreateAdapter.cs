using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using BH.Adapter;
using System.IO;
using System.Reflection;

namespace BH.UI.Alligator.Adapter
{
    public class CreateAdapter : MethodCallTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("A2D956AE-98A8-486D-A2AB-371B45F8B3AE"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Base.Properties.Resources.Adapter; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.secondary;

        public override bool ShortenBranches { get; set; } = true;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateAdapter() : base("Create Adapter", "Adapter", "Creates a specific class of Adapter", "Alligator", " Adapter") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override IEnumerable<MethodBase> GetRelevantMethods()
        {
            Type adapterType = typeof(BHoMAdapter);
            return BH.Engine.Reflection.Query.AdapterTypeList().Where(x => x.IsSubclassOf(adapterType)).OrderBy(x => x.Name).SelectMany(x => x.GetConstructors());
        }

        /*******************************************/
    }
}