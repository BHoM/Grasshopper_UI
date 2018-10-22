using System;
using Rhino;
using Rhino.Commands;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.oM.UI;
using BH.Engine.Grasshopper;
using BH.Engine.Alligator.Objects;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Templates;
using System.Windows.Forms;
using BH.UI.Basilisk.Global;
using System.Reflection;
using BH.oM.Reflection;
using Grasshopper.Kernel.Parameters;
using BH.UI.Alligator.Objects;
using BH.Engine.Reflection;
using BH.oM.Geometry;

namespace BH.UI.Alligator.Templates
{
    public abstract class CallerComponent : GH_Component, IGH_InitCodeAware, IGH_VariableParameterComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public abstract Caller Caller { get; }

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Caller.Icon_24x24; } }

        public override Guid ComponentGuid { get { return Caller.Id; } }

        public override GH_Exposure Exposure { get { return (GH_Exposure)Math.Pow(2, Caller.GroupIndex); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CallerComponent() : base()
        {
            NewInstanceGuid(System.Guid.Empty);
            PostConstructor(); // For some reason, this is not executed by Grasshopper when we just call base()
            NewInstanceGuid();

            Name = Caller.Name;
            NickName = Caller.Name;
            Description = Caller.Description;
            Category = "Alligator";
            SubCategory = Caller.Category;

            m_Accessor = new DataAccessor_GH(this);
            Caller.SetDataAccessor(m_Accessor);

            Caller.ItemSelected += (sender, e) => RefreshComponent();
        }

        /*******************************************/

        static CallerComponent()
        {
            GlobalSearchMenu.Activate();
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public void RefreshComponent()
        {
            Name = Caller.Name;
            NickName = Caller.Name;
            Description = Caller.Description;

            IGH_Attributes backup = m_attributes;
            PostConstructor();
            this.m_attributes = backup;

            this.OnAttributesChanged();
            ExpireSolution(true);
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            if (Caller == null)
                return;

            List<ParamInfo> inputs = Caller.InputParams;
            int nbNew = inputs.Count;
            int nbOld = Params.Input.Count;

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
            {
                IGH_Param newParam = ToGH_Param(inputs[i]);
                if (newParam.GetType() != Params.Input[i].GetType())
                    Params.Input[i] = newParam;
            }
                
            for (int i = nbOld - 1; i >= nbNew; i--)
                Params.UnregisterInputParameter(Params.Input[i]);

            for (int i = nbOld; i < nbNew; i++)
                Params.RegisterInputParam(ToGH_Param(inputs[i]));
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            if (Caller == null)
                return;

            List<ParamInfo> outputs = Caller.OutputParams;
            int nbNew = outputs.Count;
            int nbOld = Params.Output.Count;

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
            {
                IGH_Param newParam = ToGH_Param(outputs[i]);
                if (newParam.GetType() != Params.Output[i].GetType())
                {
                    Params.Output[i].IsolateObject();
                    Params.Output[i] = newParam;
                }
            }
                
            for (int i = nbOld - 1; i >= nbNew; i--)
                Params.UnregisterOutputParameter(Params.Output[i]);

            for (int i = nbOld; i < nbNew; i++)
                Params.RegisterOutputParam(ToGH_Param(outputs[i]));
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            m_Accessor.GH_Accessor = DA;
            Caller.Run();
            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*******************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Caller.AddToMenu(menu);
        }

        /*******************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetString("Component", Caller.Write());
            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (!base.Read(reader) || !Params.Read(reader))
                return false;

            string callerString = ""; reader.TryGetString("Component", ref callerString);
            Caller.Read(callerString);

            return true;
        }

        /*************************************/

        public virtual bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        /*************************************/

        public virtual bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        /*************************************/

        public virtual IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Grasshopper.Kernel.Parameters.Param_GenericObject();
        }

        /*************************************/

        public virtual bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        /*************************************/

        public virtual void VariableParameterMaintenance() { }


        /*************************************/
        /**** Initialisation via String   ****/
        /*************************************/

        public void SetInitCode(string code)
        {
            MethodInfo method = BH.Engine.Serialiser.Convert.FromJson(code) as MethodInfo;
            if (method != null)
                Caller.SetItem(method);
            RefreshComponent();
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private IGH_Param ToGH_Param(ParamInfo info)
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
                        {
                            param = new Param_ScriptVariable();
                            param.AttributesChanged += Param_AttributesChanged;
                        }
                            
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

        /*******************************************/

        private void Param_AttributesChanged(IGH_DocumentObject sender, GH_AttributesChangedEventArgs e)
        {
            Caller.SetDataAccessor(m_Accessor);
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private DataAccessor_GH m_Accessor = null;

        /*******************************************/
    }
}
