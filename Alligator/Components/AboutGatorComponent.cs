using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Alligator.Components
{
    public class AboutGatorComponent : GH_Component
    {
        public AboutGatorComponent()
            : base("Alligator", "Gator",
                "About Alligator",
                "Alligator", "Gator")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Execute", "E", "Execute?", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("String", "S", "String output", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool Execute = true;

            if (!DA.GetData(0, ref Execute)) return;
            if (!Execute) return;


            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            String versionStamp = ConvertVersionToDateBasedVersionStamp(version);

            List<String> stringOut = new List<string>();

            stringOut.Add("Snap!");
            stringOut.Add("Alligator - " + versionStamp);

            DA.SetDataList(0, stringOut);
  
        }


        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Alligator_Icon; }
        }


        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{2f4c7d17-d7ce-48d3-a098-19300720eccb}"); }
        }




        /// <summary>
        /// Create version Stamp based on current build version and concatinated build date and time
        /// TODO: move functionality to common utility namespace in the BHoM?
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        static public string ConvertVersionToDateBasedVersionStamp(Version version)
        {

            DateTime date = new DateTime(2000, 01, 01).AddDays(version.Build).AddSeconds(version.Revision * 2);

            String month = AddPreceedingZeroToUnitNumber(date.Month);
            String day = AddPreceedingZeroToUnitNumber(date.Day);
            String hour = AddPreceedingZeroToUnitNumber(date.Hour);
            String minute = AddPreceedingZeroToUnitNumber(date.Minute);

            String dateStamp = date.Year.ToString() + month + day + "." + hour + ":" + minute;

            return version.Major.ToString() + "." + version.Minor.ToString() + "." + dateStamp;

        }

        /// <summary>
        /// Function to ensure all integers up to '99', on conversion to string, appear as two digits with leading zero
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        static string AddPreceedingZeroToUnitNumber(int num)
        {
            if (num < 10) return "0" + num.ToString();

            return num.ToString();

        }

    }
}
