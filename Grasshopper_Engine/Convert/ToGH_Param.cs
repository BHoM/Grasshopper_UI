using BH.Engine.Alligator.Objects;
using BH.Engine.Reflection;
using BH.Engine.UI;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Reflection;
using BH.oM.UI;
using BH.UI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static IGH_Param ToGH_Param(this ParamInfo info)
        {
            UnderlyingType subType = info.DataType.UnderlyingType();
            IGH_Param param;

            switch (subType.Type.FullName)
            {
                case "System.Boolean":
                    param = new Param_Boolean();
                    break;
                case "System.Drawing.Color":
                    param = new Param_Colour();
                    break;
                case "System.DateTime":
                    param = new Param_Time();
                    break;
                case "System.Double":
                    param = new Param_Number();
                    break;
                case "System.Guid":
                    param = new Param_Guid();
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    param = new Param_Integer();
                    break;
                case "System.String":
                    param = new Param_String();
                    break;
                case "System.Type":
                    param = new Param_Type();
                    break;
                default:
                {
                    Type type = subType.Type;
                    if (typeof(IGeometry).IsAssignableFrom(type))
                        param = new Param_BHoMGeometry();
                    else if (typeof(IBHoMObject).IsAssignableFrom(type))
                        param = new Param_BHoMObject();
                    else if (typeof(IObject).IsAssignableFrom(type))
                        param = new Param_IObject();
                    else if (typeof(Enum).IsAssignableFrom(type))
                        param = new Param_Enum();
                    else
                        param = new Param_GenericObject();
                }
                break;
            }

            param.Access = (GH_ParamAccess)subType.Depth;
            param.Description = info.Description;
            param.Name = info.Name;
            param.NickName = info.Name;
            param.Optional = info.HasDefaultValue;

            try
            {
                if (info.HasDefaultValue)
                    ((dynamic)param).SetPersistentData(info.DefaultValue);
            }
            catch { }

            return param;
        }

    }
}
