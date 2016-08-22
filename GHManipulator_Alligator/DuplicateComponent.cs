using GH_IO;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Documentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;

namespace Alligator.GH_Manipulator
{
    public class DuplicateComponent : GH_Component
    {
        // Initializes a new instance of the DuplicateComponent class.
        public DuplicateComponent(): base("DuplicateComponent", "DuplicateComp","Duplicates components or clusters","Alligator", "GHManipulator"){}

        // Assigns id-number to the component
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("E8DF2FB3-1AB6-400C-B001-BF9A455D649C");
            }
        }

        // Registers all the input parameters for this component.
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("component", "component", "component or cluster used for duplication", GH_ParamAccess.item);
            pManager.AddTextParameter("group name", "groupName", "Name of the group for of components to duplicate", GH_ParamAccess.item);
            pManager.AddIntegerParameter("number of copies", "nbCopy", "how many times to copy", GH_ParamAccess.item);
            pManager.AddBooleanParameter("trigger", "trigger", "triggers the group creation", GH_ParamAccess.item);

            // Assign initial default data to the input parameters.
            Param_GenericObject param0 = (Param_GenericObject)pManager[0];
            param0.PersistentData.Append(new GH_ObjectWrapper(null));
            param0.Optional = true;
        }

        // Registers all the output parameters for this component. There are none!
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        // This is the method that actually does the work.
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Creating this component in the grasshopperdocument
            IGH_Component Component = this;
            GH_Document GrasshopperDocument = this.OnPingDocument();

            // Creating input parameters
            //object component = null;
            int nbCopy = 0;
            string groupName = null;
            bool trigger = false;

            // Getting the data from Grasshopper
            //DA.GetData<object>(0, ref component);
            DA.GetData<string>(1, ref groupName);
            DA.GetData<int>(2, ref nbCopy);
            DA.GetData<bool>(3, ref trigger);

            // If the botton is pressed it will proceed
            if (!trigger) return;

            Grasshopper.Kernel.IGH_Param selNumsInput = Component.Params.Input[0];
            IList<Grasshopper.Kernel.IGH_Param> sources = selNumsInput.Sources;
            if (!sources.Any()) return;
            IGH_DocumentObject comp = sources[0].Attributes.GetTopLevel.DocObject;

            // Gets component attributes like the bounds of the component which is used to shift 
            // the next one and get the size of the panels
            IGH_Attributes att = comp.Attributes;
            RectangleF bounds = att.Bounds;
            int vShift = (int)Math.Round(bounds.Height) + 10;
            int vStart = 30 + vShift;

            List<Guid> objectsToCopy = new List<Guid>();
            objectsToCopy.Add(comp.InstanceGuid);

            // Creating a Grasshopper Group g and assignning a nickname and color to it. 
            // Adding group g to the GrasshopperDocument
            Grasshopper.Kernel.Special.GH_Group g = new Grasshopper.Kernel.Special.GH_Group();
            g.NickName = groupName;
            g.Colour = Grasshopper.GUI.Canvas.GH_Skin.group_back;
            GrasshopperDocument.AddObject(g, false);
            List<IGH_Component> components = new List<IGH_Component>();

            // For-loop used to duplicate component and to assign properties to it (size, datalist...) 
            for (int i = 0; i < nbCopy; i++)
            {
                GH_DocumentIO documentIO = new GH_DocumentIO(GrasshopperDocument);
                documentIO.Copy(GH_ClipboardType.System, objectsToCopy);
                documentIO.Paste(GH_ClipboardType.System);

                documentIO.Document.TranslateObjects(new Size(0, vStart + i * vShift), false);
                documentIO.Document.SelectAll();
                documentIO.Document.MutateAllIds();

                GrasshopperDocument.DeselectAll();
                GrasshopperDocument.MergeDocument(documentIO.Document);
                g.AddObject(documentIO.Document.Objects[0].InstanceGuid);
            }
            GrasshopperDocument.DeselectAll();

        }

        // Function that gets parameter from the input component
        private IGH_Param getParam(IGH_DocumentObject o, int index, bool isInput)
        {
            IGH_Param result = null;

            if (o is IGH_Component)
            {
                IGH_Component p = o as IGH_Component;
                if (isInput)
                    result = p.Params.Input[index];
                else
                    result = p.Params.Output[index];
            }
            else
            {
                result = o as IGH_Param;
            }

            return result;
        }

    }
}