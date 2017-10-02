
using GH_IO;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Documentation;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BH.UI.Alligator.Base
{
    public class ComponentFromObjectList : GH_Component
    {

        public ComponentFromObjectList() : base("Components From Rhino List", "CompFromList", "Generate components from object list", "Alligator", "GHManipulator") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("967C8D64-7BA5-42AE-9256-D0C9F126D1E0");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("data list", "data", "list of objects used to create the components", GH_ParamAccess.list);
            pManager.AddTextParameter("group name", "groupName", "name to give to the group containing the created components", GH_ParamAccess.item);
            pManager.AddGenericParameter("template", "template", "panel used as template for replication and position", GH_ParamAccess.item);
            pManager.AddBooleanParameter("trigger", "trigger", "triggers the component creation", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            IGH_Component Component = this;
            GH_Document GrasshopperDocument = this.OnPingDocument();


            List<object> data = new List<object>();
            string groupName = "";
            bool trigger = false;
            object template = null;

            DA.GetDataList<object>(0, data);
            DA.GetData<string>(1, ref groupName);
            DA.GetData<object>(2, ref template);
            DA.GetData<bool>(3, ref trigger);
            

            // Trigger input 
            if (!trigger) return;

            // Taking out the position and attributes of the template panel
            Grasshopper.Kernel.IGH_Param templateInput = Component.Params.Input[2];
            IList<Grasshopper.Kernel.IGH_Param> sources = templateInput.Sources;
            if (!sources.Any()) return;
            IGH_DocumentObject templateComp = sources[0].Attributes.GetTopLevel.DocObject;
            IGH_Attributes att = templateComp.Attributes;



            // taking out the measures from the template panel and adding a shift
            RectangleF bounds = att.Bounds;
            int vShift = (int)Math.Round(bounds.Height) + 10;
            float refX = bounds.X;
            float refY = bounds.Y + 30 + vShift;

            int nbCopy = data.Count;


            // Creating a group, naming it, assigning color, adding it to the document
            Grasshopper.Kernel.Special.GH_Group g = new Grasshopper.Kernel.Special.GH_Group();
            g.NickName = groupName;
            g.Colour = Grasshopper.GUI.Canvas.GH_Skin.group_back;
            GrasshopperDocument.AddObject(g, false);

            // Putting in all the new components in the document and grouping them
            for (int i = 0; i < nbCopy; i++)
            {
                Grasshopper.Kernel.Parameters.Param_GenericObject comp = new Grasshopper.Kernel.Parameters.Param_GenericObject();
                comp.CreateAttributes();
                comp.SetPersistentData(data[i]);
                float w = comp.Attributes.Bounds.Width;
                comp.Attributes.Pivot = new PointF(refX + w / 2, refY + i * vShift);
                GrasshopperDocument.AddObject(comp, false);
                g.AddObject(comp.InstanceGuid);
            }


            GrasshopperDocument.DeselectAll();
            
        }



    }

}
