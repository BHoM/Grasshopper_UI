//using Grasshopper.Kernel;
//using System;
//using System.Collections.Generic;
//using BH.UI.Alligator.Base;
//using BH.oM.Base;
//using BH.oM.DataManipulation.Queries;
//using BH.Adapter;
//using System.Linq;
//using BH.oM.Geometry;
//using BH.Engine.Rhinoceros;
//using Grasshopper.Kernel.Data;
//using System.Windows.Forms;
//using System.IO;
//using Grasshopper.Kernel.Special;
//using Grasshopper.Kernel.Parameters;

//namespace BH.UI.Alligator.Adapter
//{
//    public class Script : GH_Component
//    {
//        /*******************************************/
//        /**** Properties                        ****/
//        /*******************************************/

//        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = null;

//        public override Guid ComponentGuid { get; } = new Guid("B112062D-3088-432E-892D-260453F1AC15");

//        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


//        /*******************************************/
//        /**** Constructors                      ****/
//        /*******************************************/

//        public Script() : base("Script", "Script", "Run a script from a file", "Alligator", " UI") { }


//        /*******************************************/
//        /**** Override Methods                  ****/
//        /*******************************************/

//        protected override void RegisterInputParams(GH_InputParamManager pManager) {}

//        /*******************************************/

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager) {}

//        /*******************************************/

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            Engine.Reflection.Compute.ClearCurrentEvents();

//            DA.DisableGapLogic();
//            if (DA.Iteration > 0)
//                return;

//            if (m_Document != null)
//            {
//                // Link inputs
//                for(int i = 0; i < Params.Input.Count(); i++)
//                {
//                    IGH_Param param = Params.Input[i];
//                    GH_ClusterInputHook hook = m_InputHooks[i];

//                    hook.ExpireSolution(false);
//                    hook.ClearData();
//                    if (!param.VolatileData.IsEmpty && !hook.AddVolatileDataTree(param.VolatileData))
//                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Volatile data merge failed");
//                }

//                // Run Document
//                m_Document.Enabled = true;
//                m_Document.NewSolution(false);
//                m_Document.Enabled = false;

//                // Link outputs
//                for (int i = 0; i < Params.Output.Count(); i++)
//                {
//                    IGH_Param param = Params.Output[i];
//                    GH_ClusterOutputHook hook = m_OutputHooks[i];

//                    param.VolatileData.Clear();
//                    param.AddVolatileDataTree(hook.VolatileData);
//                }
//            }

//            Logging.ShowEvents(this, Engine.Reflection.Query.CurrentEvents());
//        }

//        /*******************************************/

//        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
//        {
//            base.AppendAdditionalComponentMenuItems(menu);

//            if (m_Document != null)
//                return;

//            GH_Document doc = OnPingDocument();
//            string folder = Path.GetDirectoryName(doc.FilePath);
//            if (folder != null)
//            {
//                ToolStripMenuItem fileMenu = Menu_AppendItem(menu, "Select script file");

//                foreach (string file in Directory.GetFiles(folder))
//                    Menu_AppendItem(fileMenu.DropDown, file, File_Selected);
//            }
            
//        }

//        /*************************************/

//        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
//        {
//            if (m_Document != null)
//            {
//                writer.SetString("File", Path.GetFileName(m_Document.FilePath));
//            }
//            return base.Write(writer);
//        }

//        /*************************************/

//        public override bool Read(GH_IO.Serialization.GH_IReader reader)
//        {

//            string file = ""; reader.TryGetString("File", ref file);
//            if (file != null && file.Length > 0)
//                LoadDocument(Path.Combine(Path.GetDirectoryName(reader.ArchiveLocation), file));

//            //Read from the base
//            if (!base.Read(reader))
//                return false;

//            return true;
//        }

//        /*************************************/

//        private void File_Selected(object sender, EventArgs e)
//        {
//            ToolStripMenuItem item = (ToolStripMenuItem)sender;
//            LoadDocument(item.Text);
//        }

//        /*************************************/

//        private void LoadDocument(string file)
//        {
//            // Oend the document
//            GH_DocumentIO io = new GH_DocumentIO();
//            if (io.Open(file))
//                m_Document = io.Document;
//            if (m_Document == null)
//                return;

//            //Listen for change
//            FileSystemWatcher fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(file), Path.GetFileName(file));
//            fileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size;
//            fileWatcher.Changed += new FileSystemEventHandler(FileWatcher_Changed);
//            fileWatcher.EnableRaisingEvents = true;

//            // Rename the component
//            Name = Path.GetFileName(file);
//            NickName = Name;

//            // Create the inputs
//            GH_ClusterInputHook[] inputs = m_Document.ClusterInputHooks();
//            if (inputs != null)
//                m_InputHooks = inputs.ToList();
//            for (int i = 0; i < Math.Min(Params.Input.Count, m_InputHooks.Count); i++)
//            {
//                Params.Input[i].Name = m_InputHooks[i].Name;
//                Params.Input[i].NickName = m_InputHooks[i].NickName;
//                Params.Input[i].Description = m_InputHooks[i].Description;
//            }
//            for (int i = Params.Input.Count; i < m_InputHooks.Count; i++)
//            {
//                GH_ClusterInputHook input = m_InputHooks[i];
//                IGH_Param param = new Param_GenericObject { NickName = input.NickName, Name = input.NickName, Description = input.Description };
//                Params.RegisterInputParam(param);
//            }
//            for (int i = Params.Input.Count - 1; i > m_InputHooks.Count - 1; i--)
//                Params.UnregisterInputParameter(Params.Input[i]);

//            // Create the outputs
//            GH_ClusterOutputHook[] outputs = m_Document.ClusterOutputHooks();
//            if (outputs != null)
//                m_OutputHooks = outputs.ToList();
//            for (int i = 0; i < Math.Min(Params.Output.Count, m_OutputHooks.Count); i++)
//            {
//                Params.Output[i].Name = m_OutputHooks[i].Name;
//                Params.Output[i].NickName = m_OutputHooks[i].NickName;
//                Params.Output[i].Description = m_OutputHooks[i].Description;
//            }
//            for (int i = Params.Output.Count; i < m_OutputHooks.Count; i++)
//            {
//                GH_ClusterOutputHook output = m_OutputHooks[i];
//                IGH_Param param = new Param_GenericObject { NickName = output.NickName, Name = output.NickName, Description = output.Description };
//                Params.RegisterOutputParam(param);
//            }
//            for (int i = Params.Output.Count - 1; i > m_OutputHooks.Count - 1; i--)
//                Params.UnregisterOutputParameter(Params.Output[i]);

//            OnAttributesChanged();
//            ExpireSolution(true);
//        }

//        /*******************************************/

//        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
//        {
//            if (e.FullPath == m_Document.FilePath)
//                LoadDocument(e.FullPath);
//        }

//        /*******************************************/

//        private GH_Document m_Document = null;
//        private List<GH_ClusterInputHook> m_InputHooks = new List<GH_ClusterInputHook>();
//        private List<GH_ClusterOutputHook> m_OutputHooks = new List<GH_ClusterOutputHook>();
//    }
//}
