using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace SVG_Alligator
{
    public class SVG_AlligatorInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "SVGAlligator";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("33a7f1e2-3fce-4f80-b20d-9adb8c3072a4");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "BuroHappold";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
