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

namespace BH.UI.Alligator.Templates
{
    public abstract class CallerComponent : GH_Component, IGH_InitCodeAware
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

            m_Accessor = new DataAccessor_GH();
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
                Params.Input[i] = inputs[i].ToGH_Param();

            for (int i = nbOld - 1; i >= nbNew; i--)
                Params.UnregisterInputParameter(Params.Input[i]);

            for (int i = nbOld; i < nbNew; i++)
                Params.RegisterInputParam(inputs[i].ToGH_Param());
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
                Params.Output[i] = outputs[i].ToGH_Param();

            for (int i = nbOld - 1; i >= nbNew; i--)
                Params.UnregisterOutputParameter(Params.Output[i]);

            for (int i = nbOld; i < nbNew; i++)
                Params.RegisterOutputParam(outputs[i].ToGH_Param());
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

            if (Caller.Selector != null)
                Caller.Selector.AddToMenu(menu);
        }

        /*******************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (Caller.Selector != null)
                writer.SetString("Component", Caller.Selector.Write());

            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (!base.Read(reader))
                return false;

            if (Caller.Selector != null)
            {
                string callerString = ""; reader.TryGetString("Component", ref callerString);
                return Caller.Selector.Read(callerString);
            }
            else
                return false;
        }


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
        /**** Private Fields                    ****/
        /*******************************************/

        private DataAccessor_GH m_Accessor = null;


        /*******************************************/
    }
}
