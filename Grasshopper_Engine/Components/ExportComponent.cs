using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHI = BHoM.Structural.Interface;
using BHB = BHoM.Base;

namespace Grasshopper_Engine.Components
{
    /// <summary>
    /// Export component. Creates a copy of every Item about to be exported by deep cloning.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExportComponent<T> : GH_Component where T:BHB.BHoMObject
    {
        protected List<string> m_ids;
        protected List<T> m_exportedObjects;

        private static readonly string m_typeName = typeof(T).Name;
        private static readonly string m_typeNickname = typeof(T).Name[0].ToString();

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        public ExportComponent()
        { }

        public ExportComponent(string name, string nickname, string description, string category, string subcategory) : base(name, nickname, description, category, subcategory)
        {
            m_ids = new List<string>();
            m_exportedObjects = new List<T>();

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to export " +m_typeName+"s to", GH_ParamAccess.item);
            pManager.AddGenericParameter(m_typeName+"s", m_typeNickname, m_typeName + "s to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Go", "Generate " + m_typeName + "s", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[1].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Ids", "Ids", m_typeName +" Numbers", GH_ParamAccess.list);
            pManager.AddGenericParameter(m_typeName, m_typeNickname, "Exported " + m_typeName +"s", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {

                    List<T> clonedObjects = CloneObjects(DataUtils.GetGenericDataList<T>(DA, 1));

                    m_ids = null;

                    if (m_exportedObjects == null)
                        return;

                    m_exportedObjects = SetObjects(app, clonedObjects, out m_ids);
                }
            }

            DA.SetDataList(0, m_ids);
            DA.SetDataList(1, m_exportedObjects);
        }

        /// <summary>
        /// Calls the correct set method in the app
        /// </summary>
        protected abstract List<T> SetObjects(BHI.IElementAdapter app, List<T> objects, out List<string> ids);

        protected List<T> CloneObjects(List<T> objects)
        {
            if (objects == null)
                return null;

            List<T> clones = objects.Select(x => (T)x.ShallowClone()).ToList();
            clones.ForEach(x => x.CustomData = new Dictionary<string, object>(x.CustomData));
            
            return clones;
        }
    }
}
