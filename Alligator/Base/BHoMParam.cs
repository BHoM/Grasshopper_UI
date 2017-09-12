using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GH_IO.Serialization;
using BH.oM.Base;
using Grasshopper.Kernel.Data;
using BH.oM.Structural.Elements;
using System.Windows.Forms;

namespace Alligator.Base
{
    public class BHoMParam : GH_Param<GH_ObjectWrapper>
    {
        private List<BH.oM.Base.BHoMObject> m_Objects;

        public BHoMParam() : base("BH.oM Object", "BHoMObject", "Parameter storing the BH object", "Params", "Geometry", GH_ParamAccess.list)
        {
            m_Objects = new List<BH.oM.Base.BHoMObject>();
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_BH.oM_Object; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{985ADB0A-9CD7-48A7-A76D-43548D78B422}");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void CollectVolatileData_Custom()
        {
            this.AddVolatileDataList(new GH_Path(this.VolatileDataCount), m_Objects);
        }

        private void ReadObjects()
        {
            m_Objects.Clear();
            foreach (IGH_Param param in Sources)
            {
                IGH_Structure obj = param.VolatileData;
                foreach (var item in obj.AllData(true))
                {
                    GH_ObjectWrapper value = item as GH_ObjectWrapper;
                    m_Objects.Add(value.Value as BHoMObject);
                }
            }
        } 

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Clear Values", new EventHandler(ClearValues));
            Menu_AppendItem(menu, "Internalise Data", new EventHandler(InternaliseData));
            Menu_AppendSeparator(menu);
            base.AppendAdditionalMenuItems(menu);
        }

        private void InternaliseData(object sender, EventArgs e)
        {
            ReadObjects();
            this.RemoveAllSources();
            this.ExpireSolution(true);
        }

        private void ClearValues(object sender, EventArgs e)
        {
            m_Objects.Clear();
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("Objects", BH.oM.Base.BH.oMJSON.WritePackage(m_Objects));
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Objects", ref json);

            m_Objects = BH.oM.Base.BH.oMJSON.ReadPackage(json);

            return base.Read(reader);
        }
    }
}
