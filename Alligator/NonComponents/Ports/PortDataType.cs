using BH.oM.Base;
using Grasshopper.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Alligator.Base.NonComponents.Ports
{
    public class PortDataType
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_ParamAccess AccessMode { get; set; } = GH_ParamAccess.item;

        public Type DataType { get; set; } = typeof(object);



        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PortDataType() { }

        /*******************************************/

        public PortDataType(Type type)
        {
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                AccessMode = GH_ParamAccess.item;
                DataType = type;
            }
            else if (type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type)))
            {
                AccessMode = GH_ParamAccess.list;
                if (type.GenericTypeArguments.Length > 0)
                {
                    type = type.GenericTypeArguments.First();
                    PortDataType subDataType = new PortDataType(type);
                    if (subDataType.AccessMode != GH_ParamAccess.item)
                        AccessMode = GH_ParamAccess.tree;
                    DataType = subDataType.DataType;  
                }
            }
            else
            {
                if (type.IsGenericParameter)
                {
                    type = GetTypeFromGenericParameters(type);
                }
                else if (type.ContainsGenericParameters)
                {
                    Type[] newTypes = type.GetGenericArguments().Select(x => GetTypeFromGenericParameters(x)).ToArray();
                    type = type.GetGenericTypeDefinition().MakeGenericType(newTypes);
                }

                AccessMode = GH_ParamAccess.item;
                DataType = type;
            }
        }

        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static Type GetTypeFromGenericParameters(Type type)
        {
            Type[] constrains = type.GetGenericParameterConstraints();
            if (constrains.Length == 0)
                return typeof(object);
            else
                return  constrains[0];
        }

        /*******************************************/
    }
}
