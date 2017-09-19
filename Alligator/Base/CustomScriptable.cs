using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino;

namespace BH.UI.Alligator.Base
{
    public class CustomScriptable : GH_ScriptInstance
    {
        /// <summary>
        /// Initializes a new instance of the CustomScriptable class.
        /// </summary>
        /// 
        private readonly RhinoDoc RhinoDocument;
        /// <summary>Gets the Grasshopper document that owns this script.</summary>
        private readonly GH_Document GrasshopperDocument;
        /// <summary>Gets the Grasshopper script component that owns this script.</summary>
        private readonly IGH_Component Component;

        public override void AfterRunScript()
        {
            base.AfterRunScript();
        }
        public override void BeforeRunScript()
        {
            base.BeforeRunScript();
        }

        public override void InvokeRunScript(IGH_Component owner, object rhinoDocument, int iteration, List<object> inputs, IGH_DataAccess DA)
        {
            throw new NotImplementedException();
        }
    }
}