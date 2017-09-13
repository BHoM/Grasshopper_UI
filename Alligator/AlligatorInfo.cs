using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace BH.UI.Alligator
{
    public class AlligatorInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Alligator";
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
                return "BuroHappold Computational Engineering for Grasshopper";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("4e66e70a-ce50-46a9-a146-93c151e94b88");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "BuroHappold Engineering";
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
