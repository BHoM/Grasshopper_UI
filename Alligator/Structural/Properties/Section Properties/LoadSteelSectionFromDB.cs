using System;
using System.Linq;
using System.Text.RegularExpressions;
using Grasshopper.Kernel;
using Alligator.Components;
using System.Collections.Generic;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;
using Grasshopper_Engine.Components;
using System.Windows.Forms;
using Grasshopper.Kernel.Parameters;
using GH_IO.Serialization;

namespace Alligator.Structural.Properties.Section_Properties
{
    public class LoadSteelSectionFromDB : GH_Component
    {
        private ComboBox m_Types;
        private Dictionary<string,ComboBox> m_sections;
        private ComboBox m_selectedSectionType;
        private int m_selectedType;
        private int m_selectedSection;

        public LoadSteelSectionFromDB() : base("Load Steel Section", "SteelDB", "Load a steel section from database", "Structure", "Properties")
        {
            InitializeComboBoxes();
            m_selectedType = 0;
            m_selectedSection = 0;
        }

        private void InitializeComboBoxes()
        {
            m_Types = new ComboBox();
            m_sections = new Dictionary<string, ComboBox>();

            m_Types.SelectedValueChanged += TypeChanged;


            BHoM.Base.SQLAccessor accessor = new BHoM.Base.SQLAccessor(BHoM.Base.Database.SteelSection, "UK_Sections");
            m_Types.DropDownStyle = ComboBoxStyle.DropDownList;

            List<string> sections = accessor.GetDataColumn("Name").Select(x => x.ToString().TrimEnd()).ToList();

            List<string> types = new List<string>();



            foreach (string s in sections)
            {
                string[] split = Regex.Split(s, @"\s+");
                if (!types.Contains(split[0]))
                {
                    types.Add(split[0]);
                    m_Types.Items.Add(split[0]);
                    m_sections.Add(split[0], new ComboBox());
                    m_sections[split[0]].SelectedValueChanged += SectionChanged;
                    
                }


                m_sections[split[0]].Items.Add(s);
                m_sections[split[0]].SelectedIndex = 0;
            }

            m_Types.SelectedIndex = 0;

            m_selectedSectionType = m_sections[m_Types.SelectedItem.ToString()];

        }

        private void TypeChanged(object sender, EventArgs e)
        {
            RecordUndoEvent("Section Type Changed");
            m_selectedSectionType = m_sections[m_Types.SelectedItem.ToString()];
            ExpireSolution(true);
        }

        private void SectionChanged(object sender, EventArgs e)
        {
            RecordUndoEvent("Section changed");
            ExpireSolution(true);
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("2660D7D4-205F-4943-956D-60595018F919");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Section", "S", "The choosen section", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHP.SectionProperty prop = BHP.SectionProperty.LoadFromSteelSectionDB(m_sections[m_Types.SelectedItem.ToString()].SelectedItem.ToString());
            DA.SetData(0, prop);
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendCustomItem(menu, m_Types);
            Menu_AppendCustomItem(menu, m_selectedSectionType);

            base.AppendAdditionalMenuItems(menu);
        }

        public override bool Read(GH_IReader reader)
        {
            int selectedType = 0;
            int selectedIndex = 0;

            if (reader.TryGetInt32("SectionType", ref selectedType))
            {
                m_Types.SelectedIndex = selectedType;

                if (reader.TryGetInt32("Section", ref selectedIndex))
                {
                    m_sections[m_Types.SelectedItem.ToString()].SelectedIndex = selectedIndex;
                }
            }

            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("SectionType", m_Types.SelectedIndex);
            writer.SetInt32("Section", m_sections[m_Types.SelectedItem.ToString()].SelectedIndex);

            return base.Write(writer);
        }


    }
}
