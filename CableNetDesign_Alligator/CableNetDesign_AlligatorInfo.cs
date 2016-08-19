using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace CableNetDesign_Alligator
{
    public class CableNetDesign_AlligatorInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "CableNetDesignAlligator";
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
                return new Guid("112a233a-1f45-4c52-88be-ace4ff7d1933");
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
