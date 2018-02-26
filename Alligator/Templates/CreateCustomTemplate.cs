using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Parameters.Hints;
using System.Runtime.CompilerServices;
using Grasshopper.Kernel.Data;
using Grasshopper;
using BH.UI.Alligator.GeometryHints;

namespace BH.UI.Alligator.Base
{
    public abstract class CreateCustomTemplate : GH_Component, IGH_VariableParameterComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = null;

        protected virtual List<IGH_TypeHint> AvailableTypeHints { get; } = new List<IGH_TypeHint>
        {
            new GH_NullHint(),
            new GH_HintSeparator(),
            new BH_IObjectHint(),
            new BH_BHoMObjectHint(),
            new BH_BHoMGeometryHint(),
            new BH_DictionaryHint(),
            new BH_EnumHint(),
            new BH_TypeHint(),
            new GH_HintSeparator(),
            new GH_BooleanHint_CS(),
            new GH_IntegerHint_CS(),
            new GH_DoubleHint_CS(),
            new GH_StringHint_CS(),
            new GH_HintSeparator(),
            new GH_DateTimeHint(),
            new GH_ColorHint(),
            new GH_GuidHint()
        };


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        protected CreateCustomTemplate(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public virtual bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }

        /*******************************************/

        public virtual bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return (side == GH_ParameterSide.Input);
        }

        /*******************************************/

        public virtual IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            switch (side)
            {
                case GH_ParameterSide.Input:
                    return new Param_ScriptVariable
                    {
                        NickName = GH_ComponentParamServer.InventUniqueNickname("xyzuvw", this.Params.Input)
                    };
                case GH_ParameterSide.Output:
                    return new Param_GenericObject
                    {
                        NickName = GH_ComponentParamServer.InventUniqueNickname("ABCDEF", this.Params.Output)
                    };
                default:
                    return null;
            }
        }

        /*******************************************/

        public virtual bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        /*******************************************/

        public virtual void VariableParameterMaintenance()
        {
            for (int i = 0; i < Params.Input.Count; i++)
            {
                IGH_Param iGH_Param = this.Params.Input[i];
                iGH_Param.Name = iGH_Param.NickName;
                Param_ScriptVariable param_ScriptVariable = iGH_Param as Param_ScriptVariable;
                if (param_ScriptVariable != null)
                {
                    param_ScriptVariable.ShowHints = true;
                    param_ScriptVariable.Hints = this.AvailableTypeHints;
                }
            }
        }

        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            VariableParameterMaintenance();
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager) {}


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected object GetItemFromParameter(IGH_DataAccess DA, int index)
        {
            IGH_Goo data = null;
            DA.GetData<IGH_Goo>(index, ref data);
            return this.TypeCast(data, index);
        }

        /*******************************************/

        protected List<object> GetListFromParameter(IGH_DataAccess DA, int index)
        {
            List<IGH_Goo> list = new List<IGH_Goo>();
            DA.GetDataList<IGH_Goo>(index, list);
            IGH_TypeHint typeHint = ((Param_ScriptVariable)this.Params.Input[index]).TypeHint;
            List<object> list2 = new List<object>();
            int arg_3C_0 = 0;
            checked
            {
                for (int i = arg_3C_0; i < list.Count; i++)
                {
                    list2.Add(RuntimeHelpers.GetObjectValue(this.TypeCast(list[i], typeHint)));
                }
                return list2;
            }
        }

        /*******************************************/

        protected DataTree<object> GetTreeFromParameter(IGH_DataAccess DA, int index)
        {
            GH_Structure<IGH_Goo> gH_Structure = new GH_Structure<IGH_Goo>();
            DA.GetDataTree<IGH_Goo>(index, out gH_Structure);
            IGH_TypeHint typeHint = ((Param_ScriptVariable)this.Params.Input[index]).TypeHint;
            DataTree<object> dataTree = new DataTree<object>();
            int arg_3D_0 = 0;
            checked
            {
                int num = gH_Structure.PathCount - 1;
                for (int i = arg_3D_0; i <= num; i++)
                {
                    GH_Path path = gH_Structure.get_Path(i);
                    List<IGH_Goo> list = gH_Structure.Branches[i];
                    List<object> list2 = new List<object>();
                    int arg_6D_0 = 0;
                    int num2 = list.Count - 1;
                    for (int j = arg_6D_0; j <= num2; j++)
                    {
                        list2.Add(RuntimeHelpers.GetObjectValue(this.TypeCast(list[j], typeHint)));
                    }
                    dataTree.AddRange(list2, path);
                }
                return dataTree;
            }
        }

        /*******************************************/

        protected object TypeCast(IGH_Goo data, int index)
        {
            if (Params.Input[index] is Param_ScriptVariable)
            {
                Param_ScriptVariable param_ScriptVariable = (Param_ScriptVariable)this.Params.Input[index];
                return this.TypeCast(data, param_ScriptVariable.TypeHint);
            }
            else if (Params.Input[index] is Param_String)
            {
                return ((GH_String)data).Value;
            }
            else
            {
                throw new ArgumentException("The input variable " + index + " should be either a script variable or a string.");
            }
        }

        /*******************************************/

        protected object TypeCast(IGH_Goo data, IGH_TypeHint hint)
        {
            if (data == null)
            {
                return null;
            }
            if (hint == null)
            {
                return data.ScriptVariable();
            }
            object objectValue = RuntimeHelpers.GetObjectValue(data.ScriptVariable());
            object result = null;
            hint.Cast(RuntimeHelpers.GetObjectValue(objectValue), out result);
            return result;
        }

        /*******************************************/
    }
}