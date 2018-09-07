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

namespace BH.UI.Alligator.Templates
{
    public abstract class MethodCallComponent : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected abstract MethodCaller MethodCaller { get; }

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return MethodCaller.Icon_24x24; } }

        public override Guid ComponentGuid { get { return MethodCaller.Id; } }

        public override GH_Exposure Exposure { get { return (GH_Exposure)(2 ^ MethodCaller.GroupIndex); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public MethodCallComponent() : base()
        {
            NewInstanceGuid(System.Guid.Empty);
            PostConstructor(); // For some reason, this is not executed by Grasshopper when we just call base()
            NewInstanceGuid();

            Name = MethodCaller.Name();
            NickName = MethodCaller.Name();
            Description = MethodCaller.Description();
            Category = "Alligator";
            SubCategory = MethodCaller.Category();

            m_Accessor = new DataAccessor_GH();
            MethodCaller.SetDataAccessor(m_Accessor);
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            if (MethodCaller == null)
                return;

            foreach(ParamInfo param in MethodCaller.InputParams())
                pManager.AddParameter(param.ToGH_Param());
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            if (MethodCaller == null)
                return;

            foreach (ParamInfo param in MethodCaller.OutputParams())
                pManager.AddParameter(param.ToGH_Param());
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            m_Accessor.GH_Accessor = DA;
            MethodCaller.Run();
            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private DataAccessor_GH m_Accessor = null;


        /*******************************************/
    }
}
