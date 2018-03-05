using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Parameters.Hints;
using System.Runtime.CompilerServices;
using BH.oM.Base;
using RG = Rhino.Geometry;
using System.Collections;

namespace BH.UI.Alligator.Base
{
    public class CreateCustomObject : CreateCustomTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("dbd3fe50-423a-4ea4-8bc7-7ad94d1d67e9"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.CustomObject; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateCustomObject() : base("Create Custom Object", "CustomObj", "Creates a custom BHoMObject from custom inputs", "Alligator", " oM") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }

        /*******************************************/

        public override bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }

        /*******************************************/

        public override void VariableParameterMaintenance()
        {
            base.VariableParameterMaintenance();

            for (int i = 0; i < Params.Input.Count; i++)
                Params.Input[i].Description = string.Format("Object Property {0}", Params.Input[i].NickName);
        }

        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("Name", "Name", "Name of the object", GH_ParamAccess.item);
            pManager.AddScriptVariableParameter("Tags", "Tags", "Tags of the object", GH_ParamAccess.list);
            ((Param_ScriptVariable)Params.Input[0]).TypeHint = new GH_StringHint_CS();
            ((Param_ScriptVariable)Params.Input[1]).TypeHint = new GH_StringHint_CS();
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            ((Param_ScriptVariable)pManager[0]).PersistentData.Append(new GH_String(""));

            base.RegisterInputParams(pManager);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoMObject", "object", "BHoMObject", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            m_CustomObj = new oM.Base.CustomObject();

            for (int i = 0; i < Params.Input.Count; i++)
            {
                if (Params.Input[i].NickName == "Name")
                    m_CustomObj.Name = GetItemFromParameter(DA, i) as string;
                else if (Params.Input[i].NickName == "Tags")
                    m_CustomObj.Tags = new HashSet<string>(((List<object>)GetListFromParameter(DA, i)).Cast<string>());
                else
                {
                    switch (this.Params.Input[i].Access)
                    {
                        case GH_ParamAccess.item:
                            m_CustomObj.CustomData.Add(Params.Input[i].NickName, ToBHoM(RuntimeHelpers.GetObjectValue(this.GetItemFromParameter(DA, i))));
                            break;
                        case GH_ParamAccess.list:
                            m_CustomObj.CustomData.Add(Params.Input[i].NickName, ToBHoM(RuntimeHelpers.GetObjectValue(this.GetListFromParameter(DA, i))));
                            break;
                        case GH_ParamAccess.tree:
                            m_CustomObj.CustomData.Add(Params.Input[i].NickName, ToBHoM(RuntimeHelpers.GetObjectValue(this.GetTreeFromParameter(DA, i))));
                            break;
                    }
                }
                
            }

            DA.SetData(0, m_CustomObj);
        }


        /*******************************************/

        private object ToBHoM(object obj)
        {
            if (obj is IList)
            {
                return ((IList)obj).Cast<object>().Select(x => ToBHoM(x));
            }
            else if (obj.GetType().Namespace.StartsWith("Rhino"))
            {
                try
                {
                    return BH.Engine.Rhinoceros.Convert.ToBHoM(obj as dynamic);
                }
                catch (Exception e)
                {
                    return obj;
                }
            }
            else
                return obj;
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private CustomObject m_CustomObj = new CustomObject();


        /*******************************************/
    }
}